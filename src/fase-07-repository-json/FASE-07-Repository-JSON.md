# Fase 7 — Repository JSON (System.Text.Json)

## Diagrama

```
Cliente → Repository (JSON) → Arquivo books.json
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

## Implementação JsonBookRepository
```csharp
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class JsonBookRepository : IRepository<Book, int>
{
    private readonly string _path;
    private static readonly JsonSerializerOptions _opts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };
    public JsonBookRepository(string path) => _path = path;
    public Book Add(Book e) { var list = Load(); list.RemoveAll(b => b.Id == e.Id); list.Add(e); Save(list); return e; }
    public Book? GetById(int id) => Load().FirstOrDefault(b => b.Id == id);
    public IReadOnlyList<Book> ListAll() => Load();
    public bool Update(Book e) { var list = Load(); var i = list.FindIndex(b => b.Id == e.Id); if (i < 0) return false; list[i] = e; Save(list); return true; }
    public bool Remove(int id) { var list = Load(); var ok = list.RemoveAll(b => b.Id == id) > 0; if (ok) Save(list); return ok; }
    private List<Book> Load()
    {
        if (!File.Exists(_path)) return new();
        var json = File.ReadAllText(_path);
        if (string.IsNullOrWhiteSpace(json)) return new();
        return JsonSerializer.Deserialize<List<Book>>(json, _opts) ?? new();
    }
    private void Save(List<Book> list)
    {
        var json = JsonSerializer.Serialize(list, _opts);
        File.WriteAllText(_path, json);
    }
}
```

## Exemplo de uso
```csharp
var path = Path.Combine(AppContext.BaseDirectory, "books.json");
IRepository<Book, int> repo = new JsonBookRepository(path);
BookService.Register(repo, new Book(1, "Código Limpo", "Robert C. Martin"));
BookService.Register(repo, new Book(2, "Domain-Driven Design", "Eric Evans"));
var all = BookService.ListAll(repo);
foreach (var book in all)
{
    Console.WriteLine($"#{book.Id} - {book.Title} ({book.Author})");
}
```

## Testes sugeridos
- ListAll() com arquivo inexistente → lista vazia
- Add() cria arquivo e persiste; GetById() encontra
- Update() retorna false se id não existe; true quando atualiza
- Remove() exclui item e persiste lista
- Conteúdo vazio/whitespace no arquivo → lista vazia
