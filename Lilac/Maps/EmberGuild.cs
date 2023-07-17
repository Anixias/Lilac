using System.Collections.Generic;
using Lilac.Rendering;

namespace Lilac.Maps;

public sealed class EmberGuild : IMap
{
	public EmberGuild()
	{
		GuildHall = new EmberGuildTile.Basic(this)
		{
			Name = "Guild Hall",
			Description =
				"The guild hall, large and open, tables and chairs dotted throughout. Guild banners decorate the stone pillars supporting the ceiling of the tall room."
		};

		CreateMap();
	}

	public EmberGuildTile GuildHall { get; private set; }

	public string Name => "Ember Guild";
	public IColor PrimaryColor => StandardColor.Red;
	public IColor SecondaryColor => StandardColor.DarkYellow;

	private void CreateMap()
	{
		// Populate the actual tiles
	}
}

public abstract class EmberGuildTile : ITile
{
	protected readonly Dictionary<string, ITile> connections = new();

	public EmberGuildTile(EmberGuild map)
	{
		Map = map;
	}

	public IMap Map { get; }
	public abstract string Name { get; init; }
	public abstract string Description { get; init; }
	public IReadOnlyDictionary<string, ITile> Connections => connections;

	public void AddConnection(string command, ITile tile)
	{
		connections.Add(command, tile);
	}

	public void RemoveConnection(string command)
	{
		connections.Remove(command);
	}

	public sealed class Basic : EmberGuildTile
	{
		public Basic(EmberGuild map)
			: base(map)
		{
		}

		public override string Name { get; init; } = "Room";
		public override string Description { get; init; } = "An empty room.";
	}
}