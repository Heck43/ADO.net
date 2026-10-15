using System;
using OnlineStoreCheckout.Domain;
using OnlineStoreCheckout.Products;

namespace OnlineStoreCheckout.Orders
{
    /// <summary>
    /// Позиция заказа, содержащая товар и его количество.
    /// </summary>
    public sealed class OrderItem
    {
        public Product Product { get; }
        public int Quantity { get; }

        public OrderItem(Product product, int quantity)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));

            if (quantity < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    "Количество товара должно быть не меньше 1.");
            }

            Quantity = quantity;
        }

        public Money GetTotalPrice() => Product.GetUnitPrice() * Quantity;

        public decimal GetTotalWeight() => Product.GetDeliveryWeight() * Quantity;
    }
}
