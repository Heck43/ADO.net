using OnlineStoreCheckout.Domain;

namespace OnlineStoreCheckout.Payments
{
    /// <summary>
    /// Интерфейс способа оплаты заказа.
    /// </summary>
    public interface IPaymentMethod
    {
        void Pay(Money amount);
    }
}
