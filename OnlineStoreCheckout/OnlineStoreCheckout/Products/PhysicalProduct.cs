using System;
using OnlineStoreCheckout.Domain;

namespace OnlineStoreCheckout.Products
{
    /// <summary>
    /// Физический товар — хранит цену и вес, требует физической доставки.
    /// </summary>
    public sealed class PhysicalProduct : Product
    {
        public Money Price { get; }
        public decimal WeightKg { get; }

        public PhysicalProduct(string name, Money price, decimal weightKg)
            : base(name)
        {
            if (weightKg <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(weightKg),
                    "Вес физического товара должен быть больше нуля.");
            }

            Price = price;
            WeightKg = weightKg;
        }

        public override Money GetUnitPrice() => Price;

        public override bool RequiresPhysicalDelivery => true;

        public override decimal GetDeliveryWeight() => WeightKg;

        public override string GetDescription() =>
            $"{Name} (физический товар, вес: {WeightKg:0.##} кг)";
    }
}
