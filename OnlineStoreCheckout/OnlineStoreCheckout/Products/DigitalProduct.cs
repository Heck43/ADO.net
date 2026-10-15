using System;
using OnlineStoreCheckout.Domain;

namespace OnlineStoreCheckout.Products
{
    /// <summary>
    /// Цифровой товар — хранит цену и размер файла, не требует физической доставки.
    /// </summary>
    public sealed class DigitalProduct : Product
    {
        public Money Price { get; }
        public double FileSizeMb { get; }

        public DigitalProduct(string name, Money price, double fileSizeMb)
            : base(name)
        {
            if (fileSizeMb <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(fileSizeMb),
                    "Размер файла цифрового товара должен быть больше нуля.");
            }

            Price = price;
            FileSizeMb = fileSizeMb;
        }

        public override Money GetUnitPrice() => Price;

        public override bool RequiresPhysicalDelivery => false;

        public override decimal GetDeliveryWeight() => 0m;

        public override string GetDescription() =>
            $"{Name} (цифровой товар, размер файла: {FileSizeMb:0.##} МБ)";
    }
}
