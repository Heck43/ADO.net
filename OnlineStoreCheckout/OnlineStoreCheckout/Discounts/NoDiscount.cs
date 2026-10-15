using OnlineStoreCheckout.Domain;

namespace OnlineStoreCheckout.Discounts
{
    /// <summary>
    /// Скидка отсутствует.
    /// </summary>
    public sealed class NoDiscount : DiscountPolicy
    {
        public override string Name => "Без скидки";

        public override Money CalculateDiscount(Money itemsTotal) => Money.Zero;
    }
}
