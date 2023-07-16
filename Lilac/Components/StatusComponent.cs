using System.Collections.Generic;
using Lilac.Combat;
using Lilac.Rendering;

namespace Lilac.Components;

public sealed class StatusComponent : IComponent
{
	private readonly List<StatusEffect> statusEffects = new();
	public IEnumerable<StatusEffect> StatusEffects => statusEffects;

	/// <summary>
	/// Inflicts the given <see cref="StatusEffect"/>.
	/// </summary>
	/// <param name="effect">The effect to inflict.</param>
	public void Inflict(StatusEffect effect)
	{
		statusEffects.Add(effect);
		effect.OnInflicted();
	}

	/// <summary>
	/// Removes all <see cref="StatusEffect"/> instances of the type <typeparamref name="T"/>. Does not trigger
	/// expiration events.
	/// </summary>
	/// <typeparam name="T">The type of <see cref="StatusEffect"/> to remove.</typeparam>
	public void Remove<T>() where T : StatusEffect
	{
		for (var i = 0; i < statusEffects.Count; i++)
		{
			if (statusEffects[i] is not T)
				continue;
			
			statusEffects.RemoveAt(i--);
		}
	}

	/// <summary>
	/// Removes the given <see cref="StatusEffect"/> instance, if present. Does not trigger expiration events.
	/// </summary>
	/// <param name="effect">The effect to remove.</param>
	/// <returns><see langword="true"/> if the effect was removed; <see langword="false"/> otherwise.</returns>
	public bool Remove(StatusEffect effect)
	{
		return statusEffects.Remove(effect);
	}

	public void TurnStarted()
	{
		for (var i = 0; i < statusEffects.Count; i++)
		{
			var effect = statusEffects[i];
			effect.TurnsLeft--;

			if (effect.TurnsLeft <= 0)
			{
				effect.OnExpired();
				statusEffects.RemoveAt(i--);
				continue;
			}
			
			effect.OnTurnStarted();
		}
	}

	public void TurnChanged()
	{
		foreach (var effect in statusEffects)
		{
			effect.OnTurnChanged();
		}
	}

	public void TurnEnded()
	{
		foreach (var effect in statusEffects)
		{
			effect.OnTurnEnded();
		}
	}

	public void Attacked()
	{
		foreach (var effect in statusEffects)
		{
			effect.OnAttacked();
		}
	}

	public void Render()
	{
		foreach (var effect in statusEffects)
		{
			var alignmentColor = effect.Alignment switch
			{
				StatusEffectAlignment.Positive => StandardColor.Green,
				StatusEffectAlignment.Neutral  => StandardColor.Gray,
				StatusEffectAlignment.Negative => StandardColor.Red,
				_                              => StandardColor.Gray
			};

			Screen.ForegroundColor = alignmentColor;
			Screen.Write(effect.DisplayIcon);
			Screen.ResetColor();
		}
	}
}