using System.Collections.Generic;

namespace Fase11.MiniProject.Domain;

public interface IReadRepository<T, TId>
{
    T? GetById(TId id);
    IReadOnlyList<T> ListAll();
}

public interface IWriteRepository<T, TId>
{
    T Add(T entity);
    bool Update(T entity);
    bool Remove(TId id);
}

public interface IRepository<T, TId> : IReadRepository<T, TId>, IWriteRepository<T, TId>
{
}
