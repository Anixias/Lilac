namespace Lilac.Combat;

public interface IBattleMember : IRelationship
{
	delegate void EventHandler();

	bool IsDead { get; }
	bool IsUser { get; }
	Allegiance[] Allegiances { get; }
	event EventHandler? OnEndTurn;
	void EndTurn();
	void TakeTurn();
	void TurnStarted();
	void TurnEnded();
	void TurnChanged();
	int GetInitiative();
	void PrepareForBattle(Battle battle);
	string Name { get; set; }
}