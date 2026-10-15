using OnlineStoreCheckout.Domain;
using OnlineStoreCheckout.Orders;

namespace OnlineStoreCheckout.Delivery
{
    /// <summary>
    /// Абстрактный класс способа доставки.
    /// </summary>
    public abstract class DeliveryMethod
    {
        public abstract string Name { get; }
        public abstract bool IsApplicable(Order order);
        public abstract Money CalculateCost(Order order);
    }
}
