using System;
using OnlineStoreCheckout.Domain;

namespace OnlineStoreCheckout.Products
{
    /// <summary>
    /// Подписка — хранит ежемесячную цену и количество месяцев.
    /// При сроке от 12 месяцев предоставляет скидку 10%.
    /// </summary>
    public sealed class SubscriptionProduct : Product
    {
        private const int DiscountThresholdMonths = 12;
        private const decimal LongTermMultiplier = 0.90m; // скидка 10%

        public Money MonthlyPrice { get; }
        public int DurationMonths { get; }

        public SubscriptionProduct(string name, Money monthlyPrice, int durationMonths)
            : base(name)
        {
            if (durationMonths < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(durationMonths),
                    "Продолжительность подписки должна быть не менее 1 месяца.");
            }

            MonthlyPrice = monthlyPrice;
            DurationMonths = durationMonths;
        }

        public override Money GetUnitPrice()
        {
            Money baseTotal = MonthlyPrice * DurationMonths;
            return DurationMonths >= DiscountThresholdMonths
                ? baseTotal * LongTermMultiplier
                : baseTotal;
        }

        public override bool RequiresPhysicalDelivery => false;

        public override decimal GetDeliveryWeight() => 0m;

        public override string GetDescription()
        {
            string discountNote = DurationMonths >= DiscountThresholdMonths
                ? ", включая скидку 10% за срок от 12 мес."
                : string.Empty;
            return $"{Name} (подписка на {DurationMonths} мес. по {MonthlyPrice}/мес.{discountNote})";
        }
    }
}
