namespace Fase05.RepositoryInMemory;

public static class BookService
{
    public static Book Register(IRepository<Book, int> repo, Book book)
    {
        return repo.Add(book);
    }

    public static IReadOnlyList<Book> ListAll(IRepository<Book, int> repo)
    {
        return repo.ListAll();
    }
}
