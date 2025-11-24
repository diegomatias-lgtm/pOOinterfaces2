# Fase 6 — Repository CSV (persistência em arquivo)

## Diagrama

```
Cliente → Repository (CSV) → Arquivo
```

## Contrato (reaproveitado)
```csharp
public interface IRepository<T, TId>
{
    T Add(T entity);
    T? GetById(TId id);
    IReadOnlyList<T> ListAll();
    bool Update(T entity);
    bool Remove(TId id);
}
public sealed record Book(int Id, string Title, string Author);
```

## Implementação CsvBookRepository
```csharp
// Veja src/fase-06-repository-csv/CsvBookRepository.cs para implementação completa
```

## Exemplo de uso
```csharp
var path = Path.Combine(AppContext.BaseDirectory, "books.csv");
IRepository<Book, int> repo = new CsvBookRepository(path);
BookService.Register(repo, new Book(1, "Código Limpo", "Robert C. Martin"));
BookService.Register(repo, new Book(2, "Domain-Driven Design", "Eric Evans"));
var all = BookService.ListAll(repo);
foreach (var book in all)
{
    Console.WriteLine($"#{book.Id} - {book.Title} ({book.Author})");
}
```

## Testes de integração sugeridos
- Arquivo ausente, arquivo vazio
- Inserção, atualização, remoção
- Id inexistente
- Campos com vírgulas/aspas
