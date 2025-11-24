using System.Collections.Generic;
using System.Linq;

namespace Fase08.ISP;

public sealed class ReadOnlyFake : IReadRepository<Book, int>
{
    private readonly Dictionary<int, Book> _db = new() { [1] = new(1, "DDD", "Evans") };
    public Book? GetById(int id) => _db.TryGetValue(id, out var b) ? b : null;
    public IReadOnlyList<Book> ListAll() => _db.Values.ToList();
}
