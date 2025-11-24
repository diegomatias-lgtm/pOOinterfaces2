namespace Fase08.ISP;

public sealed class CatalogQuery
{
    private readonly IReadRepository<Book, int> _read;
    public CatalogQuery(IReadRepository<Book, int> read) => _read = read;
    public Book? FindById(int id) => _read.GetById(id);
    public IReadOnlyList<Book> All() => _read.ListAll();
}
