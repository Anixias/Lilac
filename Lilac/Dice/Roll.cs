using System;
using System.Collections.Generic;
using System.Linq;

namespace Lilac.Dice;

public interface IRollResult
{
	int Value { get; }
	string ToString();
	IRollResult Add(IRollResult other);
	IRollResult Subtract(IRollResult other);
	IRollResult Multiply(IRollResult other);
	IRollResult Divide(IRollResult other);
}

public readonly struct GenericRollResult : IRollResult
{
	public int Value { get; }

	public GenericRollResult(int value)
	{
		Value = value;
	}

	public IRollResult Add(IRollResult other)
	{
		return new GenericRollResult(Value + other.Value);
	}

	public IRollResult Subtract(IRollResult other)
	{
		return new GenericRollResult(Value - other.Value);
	}

	public IRollResult Multiply(IRollResult other)
	{
		return new GenericRollResult(Value * other.Value);
	}

	public IRollResult Divide(IRollResult other)
	{
		return new GenericRollResult(Value / other.Value);
	}

	public override string ToString()
	{
		return Value.ToString();
	}
}

public readonly struct DieRollResult : IRollResult
{
	public int Value { get; }
	public int RollValue { get; }

	public DieRollResult(int value, int rollValue)
	{
		Value = value;
		RollValue = rollValue;
	}

	public IRollResult Add(IRollResult other)
	{
		if (other is DieRollResult)
			// Multiple dice do not support rolled value tracking
			return new GenericRollResult(Value + other.Value);

		// Singular die and non-die value are combinable with rolled value tracking
		return new DieRollResult(Value + other.Value, RollValue);
	}

	public IRollResult Subtract(IRollResult other)
	{
		if (other is DieRollResult)
			// Multiple dice do not support rolled value tracking
			return new GenericRollResult(Value - other.Value);

		// Singular die and non-die value are combinable with rolled value tracking
		return new DieRollResult(Value - other.Value, RollValue);
	}

	public IRollResult Multiply(IRollResult other)
	{
		if (other is DieRollResult)
			// Multiple dice do not support rolled value tracking
			return new GenericRollResult(Value * other.Value);

		// Singular die and non-die value are combinable with rolled value tracking
		return new DieRollResult(Value * other.Value, RollValue);
	}

	public IRollResult Divide(IRollResult other)
	{
		if (other is DieRollResult)
			// Multiple dice do not support rolled value tracking
			return new GenericRollResult(Value / other.Value);

		// Singular die and non-die value are combinable with rolled value tracking
		return new DieRollResult(Value / other.Value, RollValue);
	}

	public override string ToString()
	{
		return $"{Value} ({RollValue})";
	}
}

public abstract class Roll
{
	public abstract IRollResult Execute();
	public abstract (IRollResult, IRollResult) Range();
	public abstract (IRollResult, IRollResult) BaseRange();
	public abstract override string ToString();

	public static Roll operator +(Roll left, Roll right)
	{
		return new Binary.Addition(left, right);
	}

	public static Roll operator -(Roll left, Roll right)
	{
		return new Binary.Subtraction(left, right);
	}

	public static Roll operator *(Roll left, Roll right)
	{
		return new Binary.Multiplication(left, right);
	}

	public static Roll operator /(Roll left, Roll right)
	{
		return new Binary.Division(left, right);
	}

	public static Roll operator +(Roll left, int right)
	{
		return new Binary.Addition(left, new Modifier(right));
	}

	public static Roll operator -(Roll left, int right)
	{
		return new Binary.Subtraction(left, new Modifier(right));
	}

	public static Roll operator *(Roll left, int right)
	{
		return new Binary.Multiplication(left, new Modifier(right));
	}

	public static Roll operator /(Roll left, int right)
	{
		return new Binary.Division(left, new Modifier(right));
	}

	public static Roll operator +(int left, Roll right)
	{
		return new Binary.Addition(new Modifier(left), right);
	}

	public static Roll operator -(int left, Roll right)
	{
		return new Binary.Subtraction(new Modifier(left), right);
	}

	public static Roll operator *(int left, Roll right)
	{
		return new Binary.Multiplication(new Modifier(left), right);
	}

	public static Roll operator /(int left, Roll right)
	{
		return new Binary.Division(new Modifier(left), right);
	}

	/// <summary>A constant roll modifier.</summary>
	public sealed class Modifier : Roll
	{
		public Modifier(int value)
		{
			Value = value;
		}

		public int Value { get; set; }

		public override IRollResult Execute()
		{
			return new GenericRollResult(Value);
		}

		public override (IRollResult, IRollResult) Range()
		{
			return (Execute(), Execute());
		}

		public override (IRollResult, IRollResult) BaseRange()
		{
			return (new GenericRollResult(0), new GenericRollResult(0));
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public static implicit operator Modifier(int value)
		{
			return new Modifier(value);
		}
	}

	/// <summary>A single die that can be rolled.</summary>
	public sealed class Die : Roll
	{
		public static readonly Die D4 = new(4);
		public static readonly Die D6 = new(6);
		public static readonly Die D8 = new(8);
		public static readonly Die D12 = new(12);
		public static readonly Die D20 = new(20);

		public Die(int sides)
		{
			Sides = sides;
		}

		public int Sides { get; }

		public override IRollResult Execute()
		{
			var value = Random.Shared.Next(1, Sides + 1);
			return new DieRollResult(value, value);
		}

		public override (IRollResult, IRollResult) Range()
		{
			var minimum = new DieRollResult(1, 1);
			var maximum = new DieRollResult(Sides, Sides);

			return (minimum, maximum);
		}

		public override (IRollResult, IRollResult) BaseRange()
		{
			return Range();
		}

		public override string ToString()
		{
			return $"d{Sides}";
		}
	}

	/// <summary>A set of dice with the same side count that can be rolled collectively.</summary>
	public sealed class Multiple : Roll
	{
		public Multiple(uint count, Die die)
		{
			Count = count;
			BaseDie = die;
		}

		public uint Count { get; }
		public Die BaseDie { get; }

		public override IRollResult Execute()
		{
			IRollResult sum = new GenericRollResult(0);
			for (var i = 0u; i < Count; i++)
				sum = sum.Add(BaseDie.Execute());

			return sum;
		}

		public override (IRollResult, IRollResult) Range()
		{
			var range = BaseDie.Range();
			if (Count == 1u)
				return range;

			return
			(
				new GenericRollResult(range.Item1.Value * (int)Count),
				new GenericRollResult(range.Item2.Value * (int)Count)
			);
		}

		public override (IRollResult, IRollResult) BaseRange()
		{
			return Range();
		}

		public override string ToString()
		{
			return $"{Count}{BaseDie}";
		}
	}

	/// <summary>
	///     A set of dice with the same side count that can be rolled collectively, keeping the specified number of
	///     highest rolls.
	/// </summary>
	public sealed class KeepHighest : Roll
	{
		public KeepHighest(uint count, uint keep, Die die)
		{
			if (keep > count)
				throw new ArgumentOutOfRangeException(nameof(keep));

			Count = count;
			Keep = keep;
			BaseDie = die;
		}

		public uint Count { get; }
		public uint Keep { get; }
		public Die BaseDie { get; }

		public override IRollResult Execute()
		{
			var rolls = new List<int>();
			for (var i = 0u; i < Count; i++)
				rolls.Add(BaseDie.Execute().Value);

			rolls.Sort();
			while (rolls.Count > Keep)
				rolls.RemoveAt(0);

			var total = rolls.Sum();
			if (Keep == 1)
				return new DieRollResult(total, total);

			return new GenericRollResult(total);
		}

		public override (IRollResult, IRollResult) Range()
		{
			var range = BaseDie.Range();
			if (Keep == 1u)
				return range;

			return
			(
				new GenericRollResult(range.Item1.Value * (int)Keep),
				new GenericRollResult(range.Item2.Value * (int)Keep)
			);
		}

		public override (IRollResult, IRollResult) BaseRange()
		{
			return Range();
		}

		public override string ToString()
		{
			return $"{Count}{BaseDie}kh{Keep}";
		}
	}

	/// <summary>
	///     A set of dice with the same side count that can be rolled collectively, keeping the specified number of lowest
	///     rolls.
	/// </summary>
	public sealed class KeepLowest : Roll
	{
		public KeepLowest(uint count, uint keep, Die die)
		{
			if (keep > count)
				throw new ArgumentOutOfRangeException(nameof(keep));

			Count = count;
			Keep = keep;
			BaseDie = die;
		}

		public uint Count { get; }
		public uint Keep { get; }
		public Die BaseDie { get; }

		public override IRollResult Execute()
		{
			var rolls = new List<int>();
			for (var i = 0u; i < Count; i++)
				rolls.Add(BaseDie.Execute().Value);

			rolls.Sort();
			rolls.Reverse();
			while (rolls.Count > Keep)
				rolls.RemoveAt(0);

			var total = rolls.Sum();
			if (Keep == 1)
				return new DieRollResult(total, total);

			return new GenericRollResult(total);
		}

		public override (IRollResult, IRollResult) Range()
		{
			var range = BaseDie.Range();
			if (Keep == 1u)
				return range;

			return
			(
				new GenericRollResult(range.Item1.Value * (int)Keep),
				new GenericRollResult(range.Item2.Value * (int)Keep)
			);
		}

		public override (IRollResult, IRollResult) BaseRange()
		{
			return Range();
		}

		public override string ToString()
		{
			return $"{Count}{BaseDie}kl{Keep}";
		}
	}

	public abstract class Binary : Roll
	{
		public Binary(Roll left, Roll right)
		{
			Left = left;
			Right = right;
		}

		public Roll Left { get; }
		public Roll Right { get; }

		public sealed class Addition : Binary
		{
			public Addition(Roll left, Roll right)
				: base(left, right)
			{
			}

			public override IRollResult Execute()
			{
				return Left.Execute().Add(Right.Execute());
			}

			public override (IRollResult, IRollResult) Range()
			{
				var leftRange = Left.Range();
				var rightRange = Right.Range();

				return (leftRange.Item1.Add(rightRange.Item1), leftRange.Item2.Add(rightRange.Item2));
			}

			public override (IRollResult, IRollResult) BaseRange()
			{
				var leftRange = Left.BaseRange();
				var rightRange = Right.BaseRange();

				return (leftRange.Item1.Add(rightRange.Item1), leftRange.Item2.Add(rightRange.Item2));
			}

			public override string ToString()
			{
				return $"{Left} + {Right}";
			}
		}

		public sealed class Subtraction : Binary
		{
			public Subtraction(Roll left, Roll right)
				: base(left, right)
			{
			}

			public override IRollResult Execute()
			{
				return Left.Execute().Subtract(Right.Execute());
			}

			public override (IRollResult, IRollResult) Range()
			{
				var leftRange = Left.Range();
				var rightRange = Right.Range();

				return (leftRange.Item1.Subtract(rightRange.Item1), leftRange.Item2.Subtract(rightRange.Item2));
			}

			public override (IRollResult, IRollResult) BaseRange()
			{
				var leftRange = Left.BaseRange();
				var rightRange = Right.BaseRange();

				return (leftRange.Item1.Subtract(rightRange.Item1), leftRange.Item2.Subtract(rightRange.Item2));
			}

			public override string ToString()
			{
				return $"{Left} - {Right}";
			}
		}

		public sealed class Multiplication : Binary
		{
			public Multiplication(Roll left, Roll right)
				: base(left, right)
			{
			}

			public override IRollResult Execute()
			{
				return Left.Execute().Multiply(Right.Execute());
			}

			public override (IRollResult, IRollResult) Range()
			{
				var leftRange = Left.Range();
				var rightRange = Right.Range();

				return (leftRange.Item1.Multiply(rightRange.Item1), leftRange.Item2.Multiply(rightRange.Item2));
			}

			public override (IRollResult, IRollResult) BaseRange()
			{
				var leftRange = Left.BaseRange();
				var rightRange = Right.BaseRange();

				return (leftRange.Item1.Multiply(rightRange.Item1), leftRange.Item2.Multiply(rightRange.Item2));
			}

			public override string ToString()
			{
				return $"{Left} * {Right}";
			}
		}

		public sealed class Division : Binary
		{
			public Division(Roll left, Roll right)
				: base(left, right)
			{
			}

			public override IRollResult Execute()
			{
				return Left.Execute().Divide(Right.Execute());
			}

			public override (IRollResult, IRollResult) Range()
			{
				var leftRange = Left.Range();
				var rightRange = Right.Range();

				return (leftRange.Item1.Divide(rightRange.Item1), leftRange.Item2.Divide(rightRange.Item2));
			}

			public override (IRollResult, IRollResult) BaseRange()
			{
				var leftRange = Left.BaseRange();
				var rightRange = Right.BaseRange();

				return (leftRange.Item1.Divide(rightRange.Item1), leftRange.Item2.Divide(rightRange.Item2));
			}

			public override string ToString()
			{
				return $"{Left} / {Right}";
			}
		}
	}
}