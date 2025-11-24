# Fase 5 — Repository InMemory (contrato + implementação em coleção)

## Diagrama

```
Cliente → Repository → Coleção InMemory
```

## Contrato
```csharp
public interface IRepository<T, TId>
{
    T Add(T entity);
    T? GetById(TId id);
    IReadOnlyList<T> ListAll();
    bool Update(T entity);
    bool Remove(TId id);
}
```

## Implementação InMemory
```csharp
public sealed class InMemoryRepository<T, TId> : IRepository<T, TId>
    where TId : notnull
{
    private readonly Dictionary<TId, T> _store = new();
    private readonly Func<T, TId> _getId;
    public InMemoryRepository(Func<T, TId> getId)
    {
        _getId = getId ?? throw new ArgumentNullException(nameof(getId));
    }
    public T Add(T entity)
    {
        var id = _getId(entity);
        _store[id] = entity;
        return entity;
    }
    public T? GetById(TId id)
    {
        return _store.TryGetValue(id, out var entity) ? entity : default;
    }
    public IReadOnlyList<T> ListAll()
    {
        return _store.Values.ToList();
    }
    public bool Update(T entity)
    {
        var id = _getId(entity);
        if (!_store.ContainsKey(id))
            return false;
        _store[id] = entity;
        return true;
    }
    public bool Remove(TId id)
    {
        return _store.Remove(id);
    }
}
```

## Domínio de exemplo
```csharp
public sealed record Book(int Id, string Title, string Author);
```

## Serviço de domínio
```csharp
public static class BookService
{
    public static Book Register(IRepository<Book, int> repo, Book book)
    {
        return repo.Add(book);
    }
    public static IReadOnlyList<Book> ListAll(IRepository<Book, int> repo)
    {
        return repo.ListAll();
    }
}
```

## Exemplo de uso
```csharp
IRepository<Book, int> repo = new InMemoryRepository<Book, int>(book => book.Id);
BookService.Register(repo, new Book(1, "Código Limpo", "Robert C. Martin"));
BookService.Register(repo, new Book(2, "Domain-Driven Design", "Eric Evans"));
var all = BookService.ListAll(repo);
foreach (var book in all)
{
    Console.WriteLine($"#{book.Id} - {book.Title} ({book.Author})");
}
```

## Testes unitários sugeridos
- Add/ListAll retorna 1 item após inserção
- GetById para id existente retorna entidade; para id ausente, null
- Update devolve true quando existe; false caso contrário
- Remove devolve true quando remove; false caso contrário
- Cliente não acessa coleções diretamente, apenas via Repository
