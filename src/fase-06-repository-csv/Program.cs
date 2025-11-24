using System;
using System.IO;

namespace Fase06.RepositoryCsv;

public static class Program
{
    public static void Main(string[] args)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "books.csv");
        IRepository<Book, int> repo = new CsvBookRepository(path);
        BookService.Register(repo, new Book(1, "Código Limpo", "Robert C. Martin"));
        BookService.Register(repo, new Book(2, "Domain-Driven Design", "Eric Evans"));
        var all = BookService.ListAll(repo);
        Console.WriteLine("Livros cadastrados (CSV):");
        foreach (var book in all)
        {
            Console.WriteLine($"#{book.Id} - {book.Title} ({book.Author})");
        }
    }
}
