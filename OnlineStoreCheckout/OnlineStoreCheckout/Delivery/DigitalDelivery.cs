using System;
using OnlineStoreCheckout.Domain;
using OnlineStoreCheckout.Orders;

namespace OnlineStoreCheckout.Delivery
{
    /// <summary>
    /// Цифровая доставка — бесплатная, подходит только для заказов без физических товаров.
    /// </summary>
    public sealed class DigitalDelivery : DeliveryMethod
    {
        public override string Name => "Цифровая доставка (по электронной почте / в личном кабинете)";

        public override bool IsApplicable(Order order)
        {
            return order != null && !order.IsEmpty && !order.HasPhysicalProducts();
        }

        public override Money CalculateCost(Order order)
        {
            if (!IsApplicable(order))
            {
                throw new InvalidOperationException(
                    "Цифровая доставка подходит только для заказов без физических товаров.");
            }

            return Money.Zero;
        }
    }
}
