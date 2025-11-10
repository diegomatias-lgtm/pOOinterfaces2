using System.Collections.Generic;

namespace PooInterface.Core.Repositories;

// Minimal repository contract used across Fases
public interface IRepository<T>
{
    void Add(T item);
    T? GetById(System.Guid id);
    IEnumerable<T> List();
    void Update(T item);
    bool Delete(System.Guid id);
}