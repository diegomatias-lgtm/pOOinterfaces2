using System;
using System.Collections.Generic;
using System.Linq;
using School.Domain.Repositories;

namespace School.Persistence.AdoNet.InMemory;

public class InMemoryRepository<T, TId> : IReadRepository<T, TId>, IWriteRepository<T, TId>
    where TId : notnull
{
    private readonly Dictionary<TId, T> _storage = new();
    private readonly Func<T, TId> _idSelector;

    public InMemoryRepository(Func<T, TId> idSelector)
    {
        _idSelector = idSelector ?? throw new ArgumentNullException(nameof(idSelector));
    }

    public T? GetById(TId id) => _storage.TryGetValue(id, out var e) ? e : default;

    public IReadOnlyList<T> ListAll() => _storage.Values.ToList().AsReadOnly();

    public IReadOnlyList<T> Find(Func<T, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        return _storage.Values.Where(predicate).ToList().AsReadOnly();
    }

    public void Add(T entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));
        var id = _idSelector(entity);
        _storage[id] = entity;
    }

    public void Update(T entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));
        var id = _idSelector(entity);
        if (!_storage.ContainsKey(id)) throw new KeyNotFoundException($"Entity with id '{id}' not found.");
        _storage[id] = entity;
    }

    public void Remove(TId id) => _storage.Remove(id);
}
