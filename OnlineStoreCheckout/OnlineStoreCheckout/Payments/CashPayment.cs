using System;
using OnlineStoreCheckout.Domain;

namespace OnlineStoreCheckout.Payments
{
    /// <summary>
    /// Оплата наличными — проверяет достаточность средств и рассчитывает сдачу.
    /// </summary>
    public sealed class CashPayment : IPaymentMethod
    {
        public Money ProvidedCash { get; }

        public CashPayment(Money providedCash)
        {
            ProvidedCash = providedCash;
        }

        public void Pay(Money amount)
        {
            if (ProvidedCash < amount)
            {
                throw new InvalidOperationException(
                    $"Недостаточно наличных для оплаты заказа. К оплате: {amount}, внесено: {ProvidedCash}.");
            }

            Money change = ProvidedCash - amount;
            Console.WriteLine(
                $"[Оплата наличными] Внесено: {ProvidedCash}, к оплате: {amount}, сдача: {change}.");
        }
    }
}
