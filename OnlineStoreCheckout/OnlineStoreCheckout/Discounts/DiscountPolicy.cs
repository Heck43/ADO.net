using System;
using OnlineStoreCheckout.Domain;
using OnlineStoreCheckout.Orders;

namespace OnlineStoreCheckout.Discounts
{
    /// <summary>
    /// Абстрактный класс политики расчёта скидки на заказ.
    /// </summary>
    public abstract class DiscountPolicy
    {
        public abstract string Name { get; }

        public Money CalculateDiscount(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            return CalculateDiscount(order.CalculateItemsTotal());
        }

        public abstract Money CalculateDiscount(Money itemsTotal);
    }
}
