using System;
using OnlineStoreCheckout.Domain;
using OnlineStoreCheckout.Orders;

namespace OnlineStoreCheckout.Delivery
{
    /// <summary>
    /// Курьерская доставка — 300 ₽ плюс 80 ₽ за каждый килограмм физического товара.
    /// Доступна только при наличии хотя бы одного физического товара.
    /// </summary>
    public sealed class CourierDelivery : DeliveryMethod
    {
        private static readonly Money BaseCost = new Money(300m);
        private static readonly Money CostPerKg = new Money(80m);

        public override string Name => "Курьерская доставка";

        public override bool IsApplicable(Order order)
        {
            return order != null && order.HasPhysicalProducts();
        }

        public override Money CalculateCost(Order order)
        {
            if (!IsApplicable(order))
            {
                throw new InvalidOperationException(
                    "Курьерская доставка доступна только для заказов с физическими товарами.");
            }

            decimal totalWeight = order.CalculateTotalWeight();
            return BaseCost + (CostPerKg * totalWeight);
        }
    }
}
