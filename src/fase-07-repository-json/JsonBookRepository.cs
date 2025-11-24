using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fase07.RepositoryJson;

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
