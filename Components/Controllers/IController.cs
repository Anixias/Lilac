using Lilac.Combat;

namespace Lilac.Components.Controllers;

public interface IController : IComponent
{
	bool IsUser { get; }
	void BeginBattle(Battle battle);
	void TakeTurn();
}