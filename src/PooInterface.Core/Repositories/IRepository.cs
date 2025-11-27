using System.Collections.Generic;

namespace PooInterface.Core.Repositories;

// Segregated repository contracts (ISP)
public interface IReadRepository<T>
{
    T? GetById(System.Guid id);
    IEnumerable<T> List();
}

public interface IWriteRepository<T>
{
    void Add(T item);
    void Update(T item);
    bool Delete(System.Guid id);
}

// Backwards-compatible aggregate interface
public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
{
}