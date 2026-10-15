using OnlineStoreCheckout.Domain;

namespace OnlineStoreCheckout.Services
{
    /// <summary>
    /// Результат расчёта оформления заказа.
    /// </summary>
    public sealed class CheckoutResult
    {
        public Money ItemsTotal { get; }
        public Money DiscountAmount { get; }
        public Money DeliveryCost { get; }
        public Money FinalTotal { get; }
        public string DiscountDescription { get; }
        public string DeliveryDescription { get; }

        public CheckoutResult(
            Money itemsTotal,
            Money discountAmount,
            Money deliveryCost,
            Money finalTotal,
            string discountDescription,
            string deliveryDescription)
        {
            ItemsTotal = itemsTotal;
            DiscountAmount = discountAmount;
            DeliveryCost = deliveryCost;
            FinalTotal = finalTotal;
            DiscountDescription = discountDescription;
            DeliveryDescription = deliveryDescription;
        }
    }
}
