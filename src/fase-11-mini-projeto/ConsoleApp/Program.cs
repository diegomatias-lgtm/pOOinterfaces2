using System;
using Fase11.MiniProject.Domain;
using Fase11.MiniProject.Persistence.InMemory;
using Fase11.MiniProject.Services;

namespace Fase11.MiniProject.ConsoleApp;

public static class Program
{
    public static void Main(string[] args)
    {
        var repo = new InMemoryRepository<Book, int>(b => b.Id);
        var svc = new CatalogService(repo, repo);

        Console.WriteLine("Demo Fase 11 — CatalogService (InMemory)");
        svc.Register(new Book(1, "Clean Code", "Robert C. Martin"));
        svc.Register(new Book(2, "Domain-Driven Design", "Eric Evans"));

        Console.WriteLine("All books:");
        foreach (var b in svc.All())
        {
            Console.WriteLine($"#{b.Id} - {b.Title} ({b.Author})");
        }

        Console.WriteLine("Renaming book 1...");
        svc.Rename(1, "Clean Code (Revised)");
        var found = svc.FindById(1);
        Console.WriteLine($"Found: #{found?.Id} - {found?.Title}");
    }
}
