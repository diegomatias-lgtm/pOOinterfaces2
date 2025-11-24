namespace Fase06.RepositoryCsv;

public static class BookService
{
    public static Book Register(IRepository<Book, int> repo, Book book)
        => repo.Add(book);

    public static IReadOnlyList<Book> ListAll(IRepository<Book, int> repo)
        => repo.ListAll();
}
