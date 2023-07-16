using System.Collections.Generic;
using Lilac.Rendering;

namespace Lilac.Maps;

public sealed class Meadows : IMap
{
	public Meadows()
	{
	}
	
    public string Name => "Meadows";
    public IColor PrimaryColor => StandardColor.Yellow;
    public IColor SecondaryColor => StandardColor.Green;
}

public abstract class MeadowsTile : ITile
{
	protected readonly Dictionary<string, ITile> connections = new();
	
	public MeadowsTile(Meadows map)
	{
		Map = map;
	}
	
    public IMap Map { get; }
	public abstract string Name { get; }
    public abstract string Description { get; }
    public IReadOnlyDictionary<string, ITile> Connections => connections;

    public void AddConnection(string command, ITile tile)
    {
        connections.Add(command, tile);
    }

    public void RemoveConnection(string command)
    {
        connections.Remove(command);
    }
	
	public sealed class EmberGuildEntrance : MeadowsTile
	{
	    public override string Name => "Ember Guild";
	    public override string Description => "The Ember Guild, a large stone and wooden structure home to the bustling and fiesty guild members famous for their display of power and courage, towers above. A large, oak door with iron reinforcements stands before you.";
	
		public EmberGuildEntrance(Meadows map)
		: base(map)
		{
		}

		public void ConnectToEmberGuild(EmberGuild emberGuild)
		{
			var tile = emberGuild.GuildHall;
			AddConnection("Go inside", tile);
			tile.AddConnection("Go outside", this);
		}
	}
}