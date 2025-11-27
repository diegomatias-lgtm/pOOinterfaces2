using System.Collections.Generic;

namespace School.Domain.Repositories;

public interface IWriteRepository<T, TId>
{
    void Add(T entity);
    void Update(T entity);
    void Remove(TId id);
}
