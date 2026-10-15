using System;
using System.Collections.Generic;
using System.Linq;
using OnlineStoreCheckout.Domain;
using OnlineStoreCheckout.Products;

namespace OnlineStoreCheckout.Orders
{
    /// <summary>
    /// Класс заказа.
    /// </summary>
    public sealed class Order
    {
        private readonly List<OrderItem> _items = new List<OrderItem>();

        public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

        public bool IsEmpty => _items.Count == 0;

        public void AddProduct(Product product)
        {
            AddProduct(product, 1);
        }

        public void AddProduct(Product product, int quantity)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (quantity < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    "Нельзя добавить товар с количеством меньше 1.");
            }

            _items.Add(new OrderItem(product, quantity));
        }

        public Money CalculateItemsTotal()
        {
            Money total = Money.Zero;
            foreach (OrderItem item in _items)
            {
                total += item.GetTotalPrice();
            }

            return total;
        }

        public bool HasPhysicalProducts()
        {
            return _items.Any(item => item.Product.RequiresPhysicalDelivery);
        }

        public decimal CalculateTotalWeight()
        {
            return _items.Sum(item => item.GetTotalWeight());
        }
    }
}
