using System;
using System.Collections.Generic;
using System.Linq;
using Lilac.Combat;
using Lilac.Entities.Creatures;

namespace Lilac.Components.Controllers;

public sealed class AIController : IController
{
	private readonly Creature creature;
	private Battle? battle;

	public AIController(Creature creature)
	{
		this.creature = creature;
	}

	public bool IsUser => false;

	public void BeginBattle(Battle battle)
	{
		this.battle = battle;
	}

	public void TakeTurn()
	{
		if (battle is null)
		{
			creature.EndTurn();
			return;
		}

		// @TODO Better AI
		// Select a random enemy, attack them, end turn

		var targets = new List<IHittable>();

		foreach (var battleMember in battle.BattleMembers)
		{
			if (battleMember == creature)
				continue;

			if (battleMember is not IHittable hittable)
				continue;

			if (creature.GetRelationship(battleMember.Allegiances.ToArray<IRelationship>()) == RelationshipState.Enemy)
				targets.Add(hittable);
		}

		if (targets.Count > 0)
		{
			var targetIndex = Random.Shared.Next(0, targets.Count);
			var target = targets[targetIndex];
			creature.Attack(target);
		}

		creature.EndTurn();
	}
}