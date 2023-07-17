using System;
using System.Collections.Generic;

namespace Lilac.Combat;

public enum RelationshipState
{
	Enemy = -1,
	Neutral,
	Ally
}

public interface IRelationship
{
	RelationshipState DefaultRelationship { get; }
	RelationshipState? InboundDefaultRelationship { get; }
	RelationshipState GetRelationship(IRelationship? allegiance);
	RelationshipState GetRelationship(IRelationship[] allegiances);
	void SetRelationship(IRelationship? allegiance, RelationshipState relationship);
	void RemoveRelationship(IRelationship allegiance);
}

public sealed class Allegiance : IRelationship
{
	public static readonly Allegiance Player = new("Player");
	public static readonly Allegiance Guard = new("Guard");
	public static readonly Allegiance Wild = new("Wild");

	public static readonly Allegiance Aggressive = new("Aggressive")
	{
		DefaultRelationship = RelationshipState.Enemy,
		InboundDefaultRelationship = RelationshipState.Enemy
	};

	private readonly Dictionary<IRelationship, RelationshipState> relationships = new();
	private RelationshipState defaultRelationship = RelationshipState.Neutral;

	static Allegiance()
	{
		Player.SetRelationship(Guard, RelationshipState.Ally);
		Guard.SetRelationship(Player, RelationshipState.Ally);
	}

	public Allegiance(string name)
	{
		Name = name;
	}

	public string Name { get; }

	public RelationshipState DefaultRelationship
	{
		get => defaultRelationship;
		init => defaultRelationship = value;
	}

	public RelationshipState? InboundDefaultRelationship { get; private init; }

	public RelationshipState GetRelationship(IRelationship? allegiance)
	{
		if (allegiance == this)
			return RelationshipState.Ally;

		if (allegiance is null)
			return defaultRelationship;

		if (relationships.TryGetValue(allegiance, out var relationship))
			return relationship;

		if (allegiance.InboundDefaultRelationship is { } inboundRelationship)
			return inboundRelationship;

		return defaultRelationship;
	}

	public RelationshipState GetRelationship(IRelationship[] allegiances)
	{
		// Returns the lowest allegiance found, or default if none found
		RelationshipState? relationship = null;
		RelationshipState? lowestInbound = null;

		foreach (var allegiance in allegiances)
		{
			if (allegiance == this)
			{
				if (relationship is not null)
					relationship = (RelationshipState)Math.Min((int)relationship, (int)RelationshipState.Ally);
				else relationship = RelationshipState.Ally;

				continue;
			}

			if (relationships.TryGetValue(allegiance, out var allegianceRelationship))
			{
				if (relationship is not null)
					relationship = (RelationshipState)Math.Min((int)relationship, (int)allegianceRelationship);
				else relationship = allegianceRelationship;
			}

			if (allegiance.InboundDefaultRelationship is { } inbound)
			{
				if (lowestInbound is not null)
					lowestInbound = (RelationshipState)Math.Min((int)lowestInbound, (int)inbound);
				else lowestInbound = inbound;
			}
		}

		return relationship ?? lowestInbound ?? defaultRelationship;
	}

	public void SetRelationship(IRelationship? allegiance, RelationshipState relationship)
	{
		if (allegiance is null)
		{
			defaultRelationship = relationship;
			return;
		}

		relationships[allegiance] = relationship;
	}

	public void RemoveRelationship(IRelationship allegiance)
	{
		if (relationships.ContainsKey(allegiance))
			relationships.Remove(allegiance);
	}
}