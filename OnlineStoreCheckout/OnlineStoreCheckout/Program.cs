using System;
using System.Text;
using OnlineStoreCheckout.Delivery;
using OnlineStoreCheckout.Discounts;
using OnlineStoreCheckout.Domain;
using OnlineStoreCheckout.Orders;
using OnlineStoreCheckout.Payments;
using OnlineStoreCheckout.Products;
using OnlineStoreCheckout.Services;

namespace OnlineStoreCheckout
{
    /// <summary>
    /// Демонстрация работы приложения.
    /// </summary>
    internal static class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("==============================================================");
            Console.WriteLine("   ДЕМОНСТРАЦИЯ ОФОРМЛЕНИЯ ЗАКАЗА В ИНТЕРНЕТ-МАГАЗИНЕ");
            Console.WriteLine("==============================================================\n");

            // Создание физического товара, цифрового товара и подписки на 12 месяцев
            Product laptop = new PhysicalProduct(
                name: "Ноутбук UltraBook 15",
                price: new Money(65000m),
                weightKg: 2.5m);

            Product antivirus = new DigitalProduct(
                name: "Антивирус Pro (лицензия)",
                price: new Money(1500m),
                fileSizeMb: 450.0);

            Product cloudSubscription = new SubscriptionProduct(
                name: "Облачное хранилище 2 ТБ",
                monthlyPrice: new Money(500m),
                durationMonths: 12);

            // Добавление товаров в заказ с использованием обеих перегрузок AddProduct
            Order order = new Order();
            order.AddProduct(laptop);             // перегрузка AddProduct(Product) — 1 шт.
            order.AddProduct(antivirus, 2);       // перегрузка AddProduct(Product, int) — 2 шт.
            order.AddProduct(cloudSubscription);  // перегрузка AddProduct(Product) — 1 шт.

            // Вывод состава заказа и стоимости каждой позиции
            Console.WriteLine("Состав заказа:");
            int index = 1;
            foreach (OrderItem item in order.Items)
            {
                Console.WriteLine(
                    $"  {index}. {item.Product.GetDescription()}\n" +
                    $"     Цена за ед.: {item.Product.GetUnitPrice()} | Кол-во: {item.Quantity} шт. | Стоимость позиции: {item.GetTotalPrice()}");
                index++;
            }

            Console.WriteLine($"  Общий вес физических товаров: {order.CalculateTotalWeight():0.##} кг\n");

            // Расчёт итога с использованием скидки 10% и курьерской доставки
            CheckoutService checkoutService = new CheckoutService();
            DiscountPolicy discount10Percent = new PercentageDiscount(10m);
            DeliveryMethod courierDelivery = new CourierDelivery();

            CheckoutResult summary = checkoutService.CalculateCheckout(
                order,
                discount10Percent,
                courierDelivery);

            // Вывод стоимости товаров, размера скидки, стоимости доставки и итоговой суммы
            Console.WriteLine("Расчёт итоговой стоимости заказа:");
            Console.WriteLine($"  Стоимость товаров:  {summary.ItemsTotal}");
            Console.WriteLine($"  Размер скидки:      {summary.DiscountAmount} ({summary.DiscountDescription})");
            Console.WriteLine($"  Стоимость доставки: {summary.DeliveryCost} ({summary.DeliveryDescription})");
            Console.WriteLine($"  Итоговая сумма:     {summary.FinalTotal}\n");

            // Выполнение оплаты через IPaymentMethod
            Console.WriteLine("Оплата заказа:");
            IPaymentMethod cardPayment = new CardPayment("**** **** **** 7788");
            cardPayment.Pay(summary.FinalTotal);

            IPaymentMethod cashPayment = new CashPayment(new Money(70000m));
            cashPayment.Pay(summary.FinalTotal);

            // -----------------------------------------------------------------
            // Обработка некорректных сценариев
            // -----------------------------------------------------------------
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("   ПРОВЕРКА ОБРАБОТКИ ОШИБОК И ИСКЛЮЧИТЕЛЬНЫХ СИТУАЦИЙ");
            Console.WriteLine("==============================================================\n");

            // Сценарий А: Некорректное количество товара
            try
            {
                Console.WriteLine("1) Попытка добавить товар с количеством 0:");
                order.AddProduct(antivirus, 0);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"   [Перехвачено исключение] {ex.Message}\n");
            }

            // Сценарий Б: Пустой заказ
            try
            {
                Console.WriteLine("2) Попытка оформить пустой заказ:");
                Order emptyOrder = new Order();
                checkoutService.CalculateCheckout(emptyOrder, new NoDiscount(), new PickupDelivery());
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"   [Перехвачено исключение] {ex.Message}\n");
            }

            // Сценарий В: Неподходящий способ доставки (цифровая доставка для заказа с физическим товаром)
            try
            {
                Console.WriteLine("3) Попытка выбрать цифровую доставку для заказа с физическим товаром:");
                checkoutService.CalculateCheckout(order, new NoDiscount(), new DigitalDelivery());
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"   [Перехвачено исключение] {ex.Message}\n");
            }

            // Сценарий В-2: Курьерская доставка для чисто цифрового заказа
            try
            {
                Console.WriteLine("4) Попытка выбрать курьерскую доставку для заказа без физических товаров:");
                Order digitalOnlyOrder = new Order();
                digitalOnlyOrder.AddProduct(antivirus);
                checkoutService.CalculateCheckout(digitalOnlyOrder, new NoDiscount(), new CourierDelivery());
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"   [Перехвачено исключение] {ex.Message}\n");
            }

            // Сценарий Г: Недостаточная сумма наличных
            try
            {
                Console.WriteLine("5) Попытка оплатить заказ наличными при недостаточной сумме:");
                IPaymentMethod insufficientCash = new CashPayment(new Money(50000m));
                insufficientCash.Pay(summary.FinalTotal);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"   [Перехвачено исключение] {ex.Message}\n");
            }
        }
    }
}
