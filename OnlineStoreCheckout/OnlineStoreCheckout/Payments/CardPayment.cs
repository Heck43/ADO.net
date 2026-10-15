using System;
using OnlineStoreCheckout.Domain;

namespace OnlineStoreCheckout.Payments
{
    /// <summary>
    /// Оплата банковской картой.
    /// </summary>
    public sealed class CardPayment : IPaymentMethod
    {
        public string CardMaskedNumber { get; }

        public CardPayment(string cardMaskedNumber = "**** **** **** 4242")
        {
            CardMaskedNumber = cardMaskedNumber;
        }

        public void Pay(Money amount)
        {
            Console.WriteLine($"[Оплата картой {CardMaskedNumber}] Успешно списано: {amount}.");
        }
    }
}
