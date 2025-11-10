using System.Collections.Concurrent;
using PooInterface.Core.Models;

namespace PooInterface.Core.Repositories;

public class InMemoryRepository : IRepository<ToDo>
{
    private readonly ConcurrentDictionary<Guid, ToDo> _store = new();

    public void Add(ToDo item)
    {
        _store[item.Id] = item;
    }

    public bool Delete(Guid id)
    {
        return _store.TryRemove(id, out _);
    }

    public ToDo? GetById(Guid id)
    {
        _store.TryGetValue(id, out var item);
        return item;
    }

    public IEnumerable<ToDo> List() => _store.Values;

    public void Update(ToDo item)
    {
        _store[item.Id] = item;
    }
}