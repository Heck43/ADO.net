using System;
using OnlineStoreCheckout.Domain;

namespace OnlineStoreCheckout.Products
{
    /// <summary>
    /// Абстрактный базовый класс товара.
    /// </summary>
    public abstract class Product
    {
        public string Name { get; }

        protected Product(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Название товара не может быть пустым.", nameof(name));
            }

            Name = name;
        }

        /// <summary>
        /// Возвращает стоимость одной единицы товара.
        /// </summary>
        public abstract Money GetUnitPrice();

        /// <summary>
        /// Определяет, требует ли товар физической доставки.
        /// </summary>
        public abstract bool RequiresPhysicalDelivery { get; }

        /// <summary>
        /// Возвращает вес одной единицы товара в килограммах для расчёта доставки.
        /// </summary>
        public abstract decimal GetDeliveryWeight();

        /// <summary>
        /// Возвращает описание характеристик товара.
        /// </summary>
        public abstract string GetDescription();
    }
}
