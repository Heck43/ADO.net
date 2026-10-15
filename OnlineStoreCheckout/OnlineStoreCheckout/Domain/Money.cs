using System;
using System.Globalization;

namespace OnlineStoreCheckout.Domain
{
    /// <summary>
    /// Структура, представляющая денежную сумму.
    /// </summary>
    public readonly struct Money : IEquatable<Money>, IComparable<Money>
    {
        private static readonly NumberFormatInfo CurrencyFormat = new NumberFormatInfo
        {
            NumberGroupSeparator = " ",
            NumberDecimalSeparator = ",",
            NumberDecimalDigits = 2,
            NumberGroupSizes = new[] { 3 }
        };

        public decimal Amount { get; }

        public Money(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    "Денежная сумма не может быть отрицательной.");
            }

            Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
        }

        public static Money Zero => new Money(0m);

        public static Money operator +(Money left, Money right)
        {
            return new Money(left.Amount + right.Amount);
        }

        public static Money operator -(Money left, Money right)
        {
            if (left.Amount < right.Amount)
            {
                throw new InvalidOperationException(
                    $"Вычитание приводит к отрицательной сумме: {left} - {right}.");
            }

            return new Money(left.Amount - right.Amount);
        }

        public static Money operator *(Money money, int multiplier)
        {
            if (multiplier < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(multiplier),
                    "Множитель не может быть отрицательным.");
            }

            return new Money(money.Amount * multiplier);
        }

        public static Money operator *(int multiplier, Money money)
        {
            return money * multiplier;
        }

        public static Money operator *(Money money, decimal multiplier)
        {
            if (multiplier < 0m)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(multiplier),
                    "Дробный множитель не может быть отрицательным.");
            }

            return new Money(money.Amount * multiplier);
        }

        public static Money operator *(decimal multiplier, Money money)
        {
            return money * multiplier;
        }

        public static Money operator *(Money money, double multiplier)
        {
            if (multiplier < 0.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(multiplier),
                    "Дробный множитель не может быть отрицательным.");
            }

            return new Money(money.Amount * (decimal)multiplier);
        }

        public static Money operator *(double multiplier, Money money)
        {
            return money * multiplier;
        }

        public static bool operator >(Money left, Money right) => left.Amount > right.Amount;
        public static bool operator <(Money left, Money right) => left.Amount < right.Amount;
        public static bool operator >=(Money left, Money right) => left.Amount >= right.Amount;
        public static bool operator <=(Money left, Money right) => left.Amount <= right.Amount;
        public static bool operator ==(Money left, Money right) => left.Equals(right);
        public static bool operator !=(Money left, Money right) => !left.Equals(right);

        public bool Equals(Money other) => Amount == other.Amount;
        public override bool Equals(object? obj) => obj is Money other && Equals(other);
        public override int GetHashCode() => Amount.GetHashCode();
        public int CompareTo(Money other) => Amount.CompareTo(other.Amount);

        public override string ToString()
        {
            return $"{Amount.ToString("N2", CurrencyFormat)} ₽";
        }
    }
}
