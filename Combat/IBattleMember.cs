namespace Lilac.Combat;

public interface IBattleMember : IRelationship
{
	delegate void EventHandler();
	event EventHandler? OnEndTurn;
	void EndTurn();
	void TakeTurn();
	void TurnStarted();
	void TurnEnded();
	void TurnChanged();
	int GetInitiative();
	void PrepareForBattle(Battle battle);
	bool IsDead { get; }
	bool IsUser { get; }
	Allegiance[] Allegiances { get; }
}