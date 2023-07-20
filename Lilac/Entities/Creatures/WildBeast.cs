using Lilac.Combat;
using Lilac.Components;
using Lilac.Components.Controllers;
using Lilac.Dice;

namespace Lilac.Entities.Creatures;

public abstract class WildBeast : Creature
{
	public WildBeast(string species, int level, int xpRate, int xpBonus)
	 : base(species, level)
	{
		AddComponent(new AIController(this));
		AddComponent(new RewardComponent
		{
			XP = xpRate * level + xpBonus
		});

		JoinAllegiance(Allegiance.Wild);
		JoinAllegiance(Allegiance.Aggressive);
	}

	public override void Render()
	{
		Render(Name, Level);
	}
}