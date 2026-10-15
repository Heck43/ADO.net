# OnlineStoreCheckout

Консольное приложение на C# (.NET 8.0), моделирующее оформление заказа в интернет-магазине.

## Структура проекта

- `OnlineStoreCheckout.sln` — файл решения Visual Studio / Rider / .NET CLI
- `OnlineStoreCheckout/`
  - `OnlineStoreCheckout.csproj` — файл проекта (.NET 8.0)
  - `Domain/Money.cs` — структура денежной суммы (`Money`) с перегрузкой операторов `+`, `-`, `*` и форматированием `1 250,00 ₽`
  - `Products/` — иерархия товаров:
    - `Product.cs` — абстрактный базовый класс товара
    - `PhysicalProduct.cs` — физический товар (цена и вес, требует доставки)
    - `DigitalProduct.cs` — цифровой товар (цена и размер файла, без физической доставки)
    - `SubscriptionProduct.cs` — подписка (ежемесячная стоимость и срок; скидка 10% при сроке от 12 месяцев)
  - `Orders/` — модели заказа:
    - `OrderItem.cs` — позиция заказа (`Product` и `Quantity`)
    - `Order.cs` — заказ с перегрузками `AddProduct(Product)` и `AddProduct(Product, int)`
  - `Discounts/` — система скидок:
    - `DiscountPolicy.cs` — абстрактный класс скидки
    - `NoDiscount.cs` — без скидки
    - `PercentageDiscount.cs` — процентная скидка (`0..100%`)
    - `FixedDiscount.cs` — фиксированная скидка (не превышает стоимость товаров в заказе)
  - `Delivery/` — способы доставки:
    - `DeliveryMethod.cs` — абстрактный класс способа доставки
    - `CourierDelivery.cs` — курьерская доставка (`300 ₽ + 80 ₽/кг`)
    - `PickupDelivery.cs` — бесплатный самовывоз
    - `DigitalDelivery.cs` — бесплатная цифровая доставка (для заказов без физических товаров)
  - `Payments/` — способы оплаты:
    - `IPaymentMethod.cs` — интерфейс оплаты
    - `CardPayment.cs` — оплата банковской картой
    - `CashPayment.cs` — оплата наличными с проверкой суммы и расчётом сдачи
  - `Services/` — оформление заказа:
    - `CheckoutResult.cs` — результат расчёта заказа
    - `CheckoutService.cs` — сервис расчёта итога (`Стоимость товаров - Скидка + Доставка`) и проведения оплаты
  - `Program.cs` — демонстрация работы всех сценариев и обработка исключений в `Main()`

## Запуск

```bash
dotnet run --project OnlineStoreCheckout/OnlineStoreCheckout.csproj
```
