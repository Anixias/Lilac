using Lilac.Combat;
using Lilac.Entities.Creatures;

namespace Lilac.Components.Controllers;

public sealed class UserController : IController
{
	private readonly Creature creature;
	private Battle? battle;

	public UserController(Creature creature)
	{
		this.creature = creature;
	}

	public bool IsUser => true;
	
	public void BeginBattle(Battle battle)
	{
		this.battle = battle;
	}
	
	public void TakeTurn()
	{
		
	}
}