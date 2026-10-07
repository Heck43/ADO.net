using System;
using System.Collections.Generic;
using System.Linq;

// Абстрактный класс (нельзя создать объект этого класса)
public abstract class publication
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public string InventoryNumber { get; set; }

    // Конструктор
    public publication(string title, string author, int year, string inventoryNumber)
    {
        Title = title;
        Author = author;
        Year = year;
        InventoryNumber = inventoryNumber;
    }

    public abstract string GetInfo();
}


// Классы-наследники
public class Book : publication
{
    public string Genre { get; set; }
    public int Pages { get; set; }
    public Book(string title, string author, int year, string inventoryNumber, string genre, int pages) : base(title, author, year, inventoryNumber)
    {
        Genre = genre;
        Pages = pages;
    }

    public override string GetInfo()
    {
        return $"Книга:\"{Title}\", автор: {Author}, {Year} г., жанр: {Genre}, {Pages} стр. Инв. № {InventoryNumber}";
    }
}


// Журнал
public class Magazine : publication
{
    public int IssueNumber { get; set; }
    public string Month { get; set; }

    public Magazine(string title, string author, int year, string inventoryNumber, int issueNumber, string month) : base(title, author, year, inventoryNumber)
    {
        IssueNumber = issueNumber;
        Month = month;
    }

    public override string GetInfo()
    {
        string authorPart = string.IsNullOrEmpty(Author) ? "" : $"Автор: {Author}, ";
        return $"Журнал: \"{Title}\", {authorPart}{Year} г., №{IssueNumber}, {Month}. Инв. № {InventoryNumber}";
    }
}

// Газета
public class Newspaper : publication
{
    public DateTime IssueDate { get; set; }

    public Newspaper(string title, string author, int year, string inventoryNumber, DateTime issueDate) : base(title, author, year, inventoryNumber)
    {
        IssueDate = issueDate;
    }

    public override string GetInfo()
    {
        return $"Газета: \"{Title}\", {Author}, {IssueDate:dd.MM.yyyy}. Инв. № {InventoryNumber}";
    }
}


// Библиотека
public class Library
{
    private List<publication> _publications = new();

    public void Add(publication publication)
    {
        _publications.Add(publication);
    }

    public void PrintCatalog()
    {
        Console.WriteLine("=== Каталог библиотеки ===");
        foreach (var publication in _publications)
        {
            Console.WriteLine(publication.GetInfo());
        }
    }

    public List<publication> FindByAuthor(string author)
    {
        return _publications.Where(p => string.Equals(p.Author, author, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}

// Точка входа в программу
class Program
{
    static void Main()
    {
        Library library = new();
        // Создаем не менее 5 объектов разных типов
        Book book1 = new Book("Война и мир", "Л. Н. Толстой", 1869, "BK-001", "роман", 1225);
        Book book2 = new Book("Преступление и наказание", "Ф. М. Достоевский", 1866, "BK-002", "роман", 672);
        Magazine magazine1 = new Magazine("Наука и жизнь", "Редакция", 2024, "MG-014", 5, "май");
        Newspaper newspaper1 = new Newspaper("Ведомости", "Редакция", 2024, "NP-003", new DateTime(2024, 5, 12));
        Newspaper newspaper2 = new Newspaper("Коммерсантъ", "ИД Коммерсантъ", 2023, "NP-004", new DateTime(2023, 11, 20));
        // Добавляем их в библиотеку
        library.Add(book1);
        library.Add(book2);
        library.Add(magazine1);
        library.Add(newspaper1);
        library.Add(newspaper2);
        // 1. Вывод каталога
        library.PrintCatalog();
        Console.WriteLine();
        // 2. Поиск по автору
        string authorToSearch = "Л. Н. Толстой";
        Console.WriteLine($"=== Поиск публикаций автора: \"{authorToSearch}\" ===");
        List<publication> found = library.FindByAuthor(authorToSearch);
        foreach (publication item in found)
        {
            Console.WriteLine(item.GetInfo());
        }

    }
}