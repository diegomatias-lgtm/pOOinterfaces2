using System.Collections.Generic;
using Fase11.MiniProject.Domain;

namespace Fase11.MiniProject.Services;

public sealed class CatalogService
{
    private readonly IReadRepository<Book, int> _read;
    private readonly IWriteRepository<Book, int> _write;

    public CatalogService(IReadRepository<Book, int> read, IWriteRepository<Book, int> write)
        => (_read, _write) = (read, write);

    public Book Register(Book b) => _write.Add(b);
    public IReadOnlyList<Book> All() => _read.ListAll();
    public Book? FindById(int id) => _read.GetById(id);

    public bool Rename(int id, string newTitle)
    {
        var b = _read.GetById(id);
        if (b is null) return false;
        return _write.Update(b with { Title = newTitle });
    }
}
