using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using Lilac.Rendering;
using System.IO;
using System.Diagnostics;

namespace Lilac.Generation;




public sealed class Graph
{
    public int ExtraEdgeCount { get; private init; }
    private List<Node> Nodes;
    private List<Edge> Edges;

    public Graph(int nodeCount, int extraEdgeCount)
    {
        ExtraEdgeCount = Math.Min(Math.Max(extraEdgeCount, 0), ((nodeCount*(nodeCount-1))/2) - (nodeCount-1));  // apply minimum/maximum edgecount

        Random rnd = new Random();

        // generate Nodes
        Nodes = new List<Node>();
        for (int i=0; i<nodeCount; i++)
            Nodes.Add(new Node(rnd.NextSingle(), rnd.NextSingle()));

        // generate all possible edges
        List<Edge> PossibleEdges = new List<Edge>();
        for (int i=1; i<Nodes.Count(); i++)
            for (int j=0; j<i; j++)
                PossibleEdges.Add(new Edge(
                    Nodes[i],
                    Nodes[j]
                ));
        PossibleEdges = PossibleEdges.OrderBy(e => e.Length()).ToList();

        // build MST (Minimum Spanning Tree)
        Edges = new List<Edge>();
        Edge edge;
        for (int idx=0; Edges.Count()+1<Nodes.Count();)
        {
            edge = PossibleEdges[idx];
            Edges.Add(edge);
            if (!isTree(edge.nodeA))
            {
                Edges.Remove(edge);
                idx++;
            } else
                PossibleEdges.Remove(edge);
        }

        // add additional edges
        Node node;
        // List<Edge> randomNodeEdges = new List<Edge>();
        while (Edges.Count()+1 < Nodes.Count() + ExtraEdgeCount)
        {
            node = Nodes[rnd.Next(Nodes.Count())];
            // randomNodeEdges.Clear();
            foreach (Edge e in PossibleEdges)
                if (object.Equals(e.nodeA, node) || object.Equals(e.nodeB, node))
                    if (!Edges.Contains(e))
                    {
                        Edges.Add(e);
                        break;
                    }
        }

        render("net", false);
    }

    // todo: if this fuction doesnt end up getting used anywhere else, integrate it into the MST algorithm
    bool isTree(Node node)
    {
        List<Node> seen = new List<Node>();
        Queue<Node> queue = new Queue<Node>();
        List<Node> connectedNodes;
        Node currentNode;
        bool common;

        queue.Enqueue(node);
        while (queue.Any())
        {
            currentNode = queue.Dequeue();
            seen.Add(currentNode);
            // get all connected nodes
            common = false;

            connectedNodes = GetConnectedNodes(currentNode);
            foreach(Node n in connectedNodes)
            {
                if (seen.Contains(n))
                    if (common)
                        return false;
                    else
                        common = true;
                else
                    queue.Enqueue(n);
            }
        }

        return true;
    }

    // todo: same as above
    public List<Node> GetConnectedNodes(Node node)
    {
        List<Node> nodes = new List<Node>();
        foreach (Edge edge in Edges)
        {
            if (object.Equals(edge.nodeA, node))
                nodes.Add(edge.nodeB);
            if (object.Equals(edge.nodeB, node))
                nodes.Add(edge.nodeA);
        }
        return nodes;
    }

    // debug
    public void render(string filename, bool positional = true)
    {
        FileStream fp = File.Open(filename + ".dot", FileMode.Create);
        StreamWriter writetext = new StreamWriter(fp);
        writetext.WriteLine($"graph {filename}{{");
        Node node;
        for (int i=0; i<Nodes.Count(); i++)
        {
            node = Nodes[i];
            writetext.WriteLine($"    {i} [label=\"Node {i}\\n{Math.Round(node.pos.X,2)} {Math.Round(node.pos.Y,2)}\" pos=\"{node.pos.X},{node.pos.Y}{(positional ? "!" : "")}\"]");
        }
        foreach (Edge edge in Edges)
        {
            writetext.WriteLine($"    {Nodes.IndexOf(edge.nodeA)} -- {Nodes.IndexOf(edge.nodeB)} [label=\"{Edges.IndexOf(edge)}\"]");
        }
        writetext.WriteLine("}");

        // requires graphviz to be installed
        Process.Start("fdp", $"-Tpng {filename}.dot -o {filename}.png");
        Process.Start("fdp", $"-Tsvg {filename}.dot -o {filename}.svg");

        writetext.Close();
    }
}






public readonly struct Node
{
    public readonly Vector2 pos;

    public Node(float x, float y)
    {
        this.pos = new Vector2(x, y);
    }

    public Node(Vector2 pos)
    {
        this.pos = pos;
    }
}

public readonly struct Edge
{
    public readonly Node nodeA;
    public readonly Node nodeB;

    public Edge(Node NodeA, Node NodeB)
    {
        this.nodeA = NodeA;
        this.nodeB = NodeB;
    }

    public float Length()
    {
        return (nodeA.pos - nodeB.pos).Length();
    }
}

