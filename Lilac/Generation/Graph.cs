using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Lilac.Generation;

public sealed class Graph
{
	private readonly List<Edge> edges = new();
	private readonly List<Node> nodes = new();

	public Graph(int nodeCount, int extraEdgeCount)
	{
		// Apply minimum/maximum Edge Count
		ExtraEdgeCount = Math.Clamp(extraEdgeCount, 0, nodeCount * (nodeCount - 1) / 2 - (nodeCount - 1));

		// Generate Nodes
		var random = new Random();
		for (var i = 0; i < nodeCount; i++)
			nodes.Add(new Node(random.NextSingle(), random.NextSingle()));

		// Generate all possible edges
		var possibleEdges = new List<Edge>();

		for (var i = 1; i < nodes.Count; i++)
			for (var j = 0; j < i; j++)
				possibleEdges.Add(new Edge(nodes[i], nodes[j]));

		possibleEdges = possibleEdges.OrderBy(e => e.Length()).ToList();

		// Build MST (Minimum Spanning Tree)
		for (var idx = 0; edges.Count + 1 < nodes.Count;)
		{
			var edge = possibleEdges[idx];
			edges.Add(edge);
			if (!IsTree(edge.nodeA))
			{
				edges.Remove(edge);
				idx++;
			}
			else
			{
				possibleEdges.Remove(edge);
			}
		}

		// Add additional edges
		while (edges.Count + 1 < nodes.Count + ExtraEdgeCount)
		{
			var node = nodes[random.Next(nodes.Count)];
			foreach (var e in possibleEdges)
				if (e.nodeA == node || e.nodeB == node)
					if (!edges.Contains(e))
					{
						edges.Add(e);
						break;
					}
		}

		Render("net", false);
	}

	private int ExtraEdgeCount { get; }

	// todo: If this function doesn't end up getting used anywhere else, integrate it into the MST algorithm
	private bool IsTree(Node node)
	{
		var seen = new List<Node>();
		var queue = new Queue<Node>();

		queue.Enqueue(node);
		while (queue.Any())
		{
			var currentNode = queue.Dequeue();
			seen.Add(currentNode);

			// Get all connected nodes
			var common = false;

			var connectedNodes = GetConnectedNodes(currentNode);
			foreach (var n in connectedNodes)
				if (seen.Contains(n))
				{
					if (common)
						return false;

					common = true;
				}
				else
				{
					queue.Enqueue(n);
				}
		}

		return true;
	}

	// todo: Same as above
	private List<Node> GetConnectedNodes(Node node)
	{
		var connectedNodes = new List<Node>();
		foreach (var edge in edges)
		{
			if (edge.nodeA == node)
				connectedNodes.Add(edge.nodeB);

			if (edge.nodeB == node)
				connectedNodes.Add(edge.nodeA);
		}

		return connectedNodes;
	}

	// debug
	private void Render(string filename, bool isPositional = true)
	{
		var fileStream = File.Open($"{filename}.dot", FileMode.Create);
		var streamWriter = new StreamWriter(fileStream);
		streamWriter.WriteLine($"graph {filename}{{");

		for (var i = 0; i < nodes.Count; i++)
		{
			var node = nodes[i];
			var nodeLabel = $"Node {i}\\n{Math.Round(node.position.X, 2)} {Math.Round(node.position.Y, 2)}";
			var nodePosition = $"{node.position.X},{node.position.Y}{(isPositional ? "!" : "")}";
			var nodeLine = $"    {i} [label=\"{nodeLabel}\" pos=\"{nodePosition}\"]";
			streamWriter.WriteLine(nodeLine);
		}

		foreach (var edge in edges)
		{
			var nodeAIndex = nodes.IndexOf(edge.nodeA);
			var nodeBIndex = nodes.IndexOf(edge.nodeB);
			var edgeIndex = edges.IndexOf(edge);
			var edgeLine = $"    {nodeAIndex} -- {nodeBIndex} [label=\"{edgeIndex}\"]";
			streamWriter.WriteLine(edgeLine);
		}

		streamWriter.WriteLine("}");

		// Requires graphviz to be installed
		Process.Start("fdp", $"-Tpng {filename}.dot -o {filename}.png");
		Process.Start("fdp", $"-Tsvg {filename}.dot -o {filename}.svg");

		streamWriter.Close();
	}
}

public readonly struct Node : IEquatable<Node>
{
	public readonly Vector2 position;

	public Node(float x, float y)
	{
		position = new Vector2(x, y);
	}

	public Node(Vector2 position)
	{
		this.position = position;
	}

	public static bool operator ==(Node left, Node right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(Node left, Node right)
	{
		return !(left == right);
	}

	#region IEquatable<Node> members

	public bool Equals(Node other)
	{
		return position.Equals(other.position);
	}

	public override bool Equals(object? obj)
	{
		return obj is Node other && Equals(other);
	}

	public override int GetHashCode()
	{
		return position.GetHashCode();
	}

	#endregion
}

public readonly struct Edge
{
	public readonly Node nodeA;
	public readonly Node nodeB;

	public Edge(Node nodeA, Node nodeB)
	{
		this.nodeA = nodeA;
		this.nodeB = nodeB;
	}

	public float Length()
	{
		return (nodeA.position - nodeB.position).Length();
	}
}