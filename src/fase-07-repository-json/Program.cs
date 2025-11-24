using System;
using System.IO;

namespace Fase07.RepositoryJson;

public static class Program
{
    public static void Main(string[] args)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "books.json");
        IRepository<Book, int> repo = new JsonBookRepository(path);
        BookService.Register(repo, new Book(1, "Código Limpo", "Robert C. Martin"));
        BookService.Register(repo, new Book(2, "Domain-Driven Design", "Eric Evans"));
        var all = BookService.ListAll(repo);
        Console.WriteLine("Livros cadastrados (JSON):");
        foreach (var book in all)
        {
            Console.WriteLine($"#{book.Id} - {book.Title} ({book.Author})");
        }
    }
}
