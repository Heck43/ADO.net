using System;
using OnlineStoreCheckout.Domain;
using OnlineStoreCheckout.Orders;

namespace OnlineStoreCheckout.Delivery
{
    /// <summary>
    /// Самовывоз — бесплатный, доступен только при наличии хотя бы одного физического товара.
    /// </summary>
    public sealed class PickupDelivery : DeliveryMethod
    {
        public override string Name => "Самовывоз";

        public override bool IsApplicable(Order order)
        {
            return order != null && order.HasPhysicalProducts();
        }

        public override Money CalculateCost(Order order)
        {
            if (!IsApplicable(order))
            {
                throw new InvalidOperationException(
                    "Самовывоз доступен только для заказов с физическими товарами.");
            }

            return Money.Zero;
        }
    }
}
