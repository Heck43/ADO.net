using System;
using OnlineStoreCheckout.Domain;

namespace OnlineStoreCheckout.Discounts
{
    /// <summary>
    /// Скидка в процентах (от 0 до 100).
    /// </summary>
    public sealed class PercentageDiscount : DiscountPolicy
    {
        public decimal Percentage { get; }

        public PercentageDiscount(decimal percentage)
        {
            if (percentage < 0m || percentage > 100m)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(percentage),
                    "Процент скидки должен находиться в диапазоне от 0 до 100.");
            }

            Percentage = percentage;
        }

        public override string Name => $"Скидка {Percentage:0.##}%";

        public override Money CalculateDiscount(Money itemsTotal)
        {
            return itemsTotal * (Percentage / 100m);
        }
    }
}
