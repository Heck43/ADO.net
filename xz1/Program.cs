using System;
using System.Collections.Generic;
using System.Linq;

// Модель записи в базе данных
record UserRecord(int Id, string Name, string Role, DateTime CreatedAt);

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================");
        Console.WriteLine("  🚀 Демонстрационная мини-База Данных  ");
        Console.WriteLine("        Платформа: .NET 8 (C# 12)       ");
        Console.WriteLine("========================================\n");
        Console.ResetColor();

        // Коллекция записей (используем современный синтаксис C# 12: collection expressions)
        List<UserRecord> database = [
            new(1, "Алексей Иванов", "Администратор", DateTime.Now.AddDays(-10)),
            new(2, "Мария Смирнова", "Разработчик C#", DateTime.Now.AddDays(-5)),
            new(3, "Дмитрий Кузнецов", "Тестировщик", DateTime.Now.AddDays(-2)),
            new(4, "Елена Попова", "DevOps инженер", DateTime.Now.AddHours(-12))
        ];

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Меню действий:");
            Console.ResetColor();
            Console.WriteLine(" [1] Показать все записи");
            Console.WriteLine(" [2] Найти запись по имени");
            Console.WriteLine(" [3] Добавить нового пользователя");
            Console.WriteLine(" [4] Показать статистику (LINQ)");
            Console.WriteLine(" [0] Выход");
            Console.Write("\nВыберите пункт (0-4): ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    PrintAll(database);
                    break;
                case "2":
                    Search(database);
                    break;
                case "3":
                    AddUser(database);
                    break;
                case "4":
                    ShowStats(database);
                    break;
                case "0":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Работа завершена. До встречи!");
                    Console.ResetColor();
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неизвестная команда. Попробуйте еще раз.\n");
                    Console.ResetColor();
                    break;
            }
        }
    }

    static void PrintAll(List<UserRecord> db)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{"ID",-5} | {"ФИО",-20} | {"Должность",-18} | {"Дата создания"}");
        Console.WriteLine(new string('-', 68));
        Console.ResetColor();

        foreach (var user in db)
        {
            Console.WriteLine($"{user.Id,-5} | {user.Name,-20} | {user.Role,-18} | {user.CreatedAt:dd.MM.yyyy HH:mm}");
        }
        Console.WriteLine();
    }

    static void Search(List<UserRecord> db)
    {
        Console.Write("Введите часть имени для поиска: ");
        string query = Console.ReadLine() ?? "";

        var results = db.Where(u => u.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

        if (results.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ничего не найдено.\n");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Найдено записей: {results.Count}");
            Console.ResetColor();
            PrintAll(results);
        }
    }

    static void AddUser(List<UserRecord> db)
    {
        Console.Write("Введите имя нового сотрудника: ");
        string name = Console.ReadLine() ?? "Без имени";

        Console.Write("Введите должность: ");
        string role = Console.ReadLine() ?? "Сотрудник";

        int newId = db.Count > 0 ? db.Max(u => u.Id) + 1 : 1;
        db.Add(new(newId, name, role, DateTime.Now));

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Успешно добавлен: ID {newId} - {name} ({role})\n");
        Console.ResetColor();
    }

    static void ShowStats(List<UserRecord> db)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("--- Статистика базы данных ---");
        Console.WriteLine($"Всего записей: {db.Count}");
        Console.WriteLine($"Уникальных ролей: {db.Select(u => u.Role).Distinct().Count()}");
        Console.WriteLine($"Самый новый пользователь: {db.OrderByDescending(u => u.CreatedAt).FirstOrDefault()?.Name}");
        Console.ResetColor();
        Console.WriteLine();
    }
}
