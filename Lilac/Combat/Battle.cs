using System.Collections.Generic;
using System.Linq;
using Lilac.Components;
using Lilac.Entities;
using Lilac.Rendering;

namespace Lilac.Combat;

public sealed class Battle
{
	public delegate void EventHandler();

	private List<IBattleMember> battleMembers = new();
	private int currentTurn;

	public IReadOnlyList<IBattleMember> BattleMembers => battleMembers;

	public IBattleMember? CurrentBattleMember =>
		currentTurn >= 0 && currentTurn < battleMembers.Count ? battleMembers[currentTurn] : null;

	public event EventHandler? OnTurnChanged;

	public void Begin()
	{
		currentTurn = 0;
		OnTurnChanged?.Invoke();
		StartTurn();
	}

	public void AddBattleMember(IBattleMember battleMember)
	{
		if (battleMembers.Contains(battleMember))
			return;

		battleMember.PrepareForBattle(this);
		battleMembers.Add(battleMember);
		battleMembers = battleMembers.OrderByDescending(m => m.GetInitiative()).ToList();

		if (battleMembers.Count > 1)
		{
			var battleMemberTurn = battleMembers.FindIndex(m => ReferenceEquals(m, battleMember));
			if (battleMemberTurn <= currentTurn)
				currentTurn++;
		}
		else
		{
			currentTurn = 0;
		}
	}

	public void RemoveBattleMember(IBattleMember battleMember)
	{
		var battleMemberTurn = battleMembers.FindIndex(m => ReferenceEquals(m, battleMember));
		if (battleMemberTurn < 0)
			return;

		if (battleMemberTurn < currentTurn)
			currentTurn--;

		battleMembers.Remove(battleMember);

		if (battleMembers.Count <= 0)
			currentTurn = 0;
	}

	private void StartTurn()
	{
		if (CurrentBattleMember is null)
			return;

		CurrentBattleMember.TurnStarted();
		CurrentBattleMember.OnEndTurn += Advance;
	}

	private void EndTurn()
	{
		if (CurrentBattleMember is null)
			return;

		CurrentBattleMember.TurnEnded();
		CurrentBattleMember.OnEndTurn -= Advance;
	}

	public void Advance()
	{
		if (battleMembers.Count <= 0)
		{
			currentTurn = 0;
			return;
		}

		EndTurn();

		do
		{
			currentTurn = (currentTurn + 1) % battleMembers.Count;
		} while (CurrentBattleMember?.IsDead ?? false);

		foreach (var member in battleMembers) member.TurnChanged();

		StartTurn();
		OnTurnChanged?.Invoke();
	}

	public void Render()
	{
		for (var i = 0; i < battleMembers.Count; i++)
		{
			Screen.ResetColor();
			var battleMember = battleMembers[i];

			Screen.ForegroundColor = battleMember.IsDead ? StandardColor.DarkGray : StandardColor.Green;
			Screen.Strikethrough = battleMember.IsDead;

			Screen.Write(i == currentTurn ? " -> " : battleMember.IsDead ? "  X " : "    ");
			Screen.ResetColor();

			if (battleMember.IsDead)
				Screen.ForegroundColor = StandardColor.DarkGray;

			if (battleMember is not Entity entity)
			{
				Screen.WriteLine("Unknown");
				continue;
			}

			entity.Render();
			if (entity.GetComponent<HealthComponent>() is not { } healthComponent)
			{
				Screen.WriteLine();
				continue;
			}

			while (Screen.CursorLeft < 20)
				Screen.Write(" ");

			Drawing.DrawBar(8, healthComponent.Percent, StandardColor.DarkRed);

			if (entity.GetComponent<StatusComponent>() is { } statusComponent)
			{
				Screen.Write(" ");
				statusComponent.Render();
			}

			Screen.WriteLine();
		}

		Screen.Strikethrough = false;
	}
}