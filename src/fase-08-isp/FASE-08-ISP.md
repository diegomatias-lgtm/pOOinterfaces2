# Fase 8 — ISP (Interface Segregation Principle)

## Diagnóstico da interface "gorda"

Antes:
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

- Clientes que só leem (ex: consultas) dependem de métodos de escrita que não usam.
- Clientes que só escrevem (ex: comandos) dependem de métodos de leitura que não usam.

## Contratos segregados (ISP)

Depois:
```csharp
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
```

## Pontos de uso refatorados

Consumidor que só lê:
```csharp
public sealed class CatalogQuery
{
    private readonly IReadRepository<Book, int> _read;
    public CatalogQuery(IReadRepository<Book, int> read) => _read = read;
    public Book? FindById(int id) => _read.GetById(id);
    public IReadOnlyList<Book> All() => _read.ListAll();
}
```

## Dublê mínimo (somente leitura)
```csharp
file sealed class ReadOnlyFake : IReadRepository<Book, int>
{
    private readonly Dictionary<int, Book> _db = new() { [1] = new(1, "DDD", "Evans") };
    public Book? GetById(int id) => _db.TryGetValue(id, out var b) ? b : null;
    public IReadOnlyList<Book> ListAll() => _db.Values.ToList();
}
```

## Nota de design
- Segregação reduz acoplamento e simplifica testes.
- Cada consumidor depende apenas do que realmente usa.
- Dublês mínimos são mais fáceis de manter.

## Resultados de teste
- Todos os testes verdes após refatoração.
