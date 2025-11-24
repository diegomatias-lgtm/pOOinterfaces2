using System;

namespace Fase05.RepositoryInMemory;

public static class Program
{
    public static void Main(string[] args)
    {
        IRepository<Book, int> repo = new InMemoryRepository<Book, int>(book => book.Id);
        BookService.Register(repo, new Book(1, "Código Limpo", "Robert C. Martin"));
        BookService.Register(repo, new Book(2, "Domain-Driven Design", "Eric Evans"));
        var all = BookService.ListAll(repo);
        Console.WriteLine("Livros cadastrados:");
        foreach (var book in all)
        {
            Console.WriteLine($"#{book.Id} - {book.Title} ({book.Author})");
        }
    }
}
