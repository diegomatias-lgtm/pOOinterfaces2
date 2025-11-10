# ISP Demonstration

This folder explains the Interface Segregation Principle applied to repositories.

- Before: a monolithic IRepository<T> with many methods.
- After: separate small interfaces like IReadRepository<T> and IWriteRepository<T>.

Example:

- IReadRepository<T>: T? GetById(Guid id); IEnumerable<T> List();
- IWriteRepository<T>: void Add(T item); void Update(T item); bool Delete(Guid id);