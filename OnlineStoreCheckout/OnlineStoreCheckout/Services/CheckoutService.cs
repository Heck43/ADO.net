using System;
using OnlineStoreCheckout.Delivery;
using OnlineStoreCheckout.Discounts;
using OnlineStoreCheckout.Domain;
using OnlineStoreCheckout.Orders;
using OnlineStoreCheckout.Payments;

namespace OnlineStoreCheckout.Services
{
    /// <summary>
    /// Сервис оформления заказа и расчёта итоговой стоимости.
    /// </summary>
    public sealed class CheckoutService
    {
        public CheckoutResult CalculateCheckout(
            Order order,
            DiscountPolicy discountPolicy,
            DeliveryMethod deliveryMethod)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            if (discountPolicy == null)
            {
                throw new ArgumentNullException(nameof(discountPolicy));
            }

            if (deliveryMethod == null)
            {
                throw new ArgumentNullException(nameof(deliveryMethod));
            }

            if (order.IsEmpty)
            {
                throw new InvalidOperationException("Невозможно оформить пустой заказ.");
            }

            if (!deliveryMethod.IsApplicable(order))
            {
                throw new InvalidOperationException(
                    $"Выбранный способ доставки «{deliveryMethod.Name}» не подходит для содержимого данного заказа.");
            }

            Money itemsTotal = order.CalculateItemsTotal();
            Money discountAmount = discountPolicy.CalculateDiscount(order);
            Money deliveryCost = deliveryMethod.CalculateCost(order);

            // Формула расчёта: Итого = Стоимость товаров - Скидка + Доставка
            Money finalTotal = (itemsTotal - discountAmount) + deliveryCost;

            return new CheckoutResult(
                itemsTotal,
                discountAmount,
                deliveryCost,
                finalTotal,
                discountPolicy.Name,
                deliveryMethod.Name);
        }

        public CheckoutResult ProcessOrder(
            Order order,
            DiscountPolicy discountPolicy,
            DeliveryMethod deliveryMethod,
            IPaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
            {
                throw new ArgumentNullException(nameof(paymentMethod));
            }

            CheckoutResult result = CalculateCheckout(order, discountPolicy, deliveryMethod);
            paymentMethod.Pay(result.FinalTotal);
            return result;
        }
    }
}
