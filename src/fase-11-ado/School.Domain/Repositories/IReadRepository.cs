using System;
using System.Collections.Generic;

namespace School.Domain.Repositories;

public interface IReadRepository<T, TId>
{
    T? GetById(TId id);
    IReadOnlyList<T> ListAll();
    IReadOnlyList<T> Find(Func<T, bool> predicate);
}
