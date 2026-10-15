using System;
using OnlineStoreCheckout.Domain;

namespace OnlineStoreCheckout.Discounts
{
    /// <summary>
    /// Скидка в фиксированной денежной сумме (не может превышать стоимость товаров в заказе).
    /// </summary>
    public sealed class FixedDiscount : DiscountPolicy
    {
        public Money DiscountAmount { get; }

        public FixedDiscount(Money discountAmount)
        {
            DiscountAmount = discountAmount;
        }

        public override string Name => $"Фиксированная скидка ({DiscountAmount})";

        public override Money CalculateDiscount(Money itemsTotal)
        {
            if (DiscountAmount > itemsTotal)
            {
                throw new InvalidOperationException(
                    $"Фиксированная скидка ({DiscountAmount}) не может превышать стоимость товаров в заказе ({itemsTotal}).");
            }

            return DiscountAmount;
        }
    }
}
