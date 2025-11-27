using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fase11.MiniProject.Domain;

namespace Fase11.MiniProject.Persistence.Json;

public sealed class JsonBookRepository : IRepository<Book, int>
{
    private readonly string _path;
    private static readonly JsonSerializerOptions _opts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    public JsonBookRepository(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("path", nameof(path));
        _path = path;
    }

    public Book Add(Book entity)
    {
        var list = Load();
        list.RemoveAll(b => b.Id == entity.Id);
        list.Add(entity);
        Save(list);
        return entity;
    }

    public Book? GetById(int id) => Load().FirstOrDefault(b => b.Id == id);

    public IReadOnlyList<Book> ListAll() => Load();

    public bool Update(Book entity)
    {
        var list = Load();
        var idx = list.FindIndex(b => b.Id == entity.Id);
        if (idx < 0) return false;
        list[idx] = entity;
        Save(list);
        return true;
    }

    public bool Remove(int id)
    {
        var list = Load();
        var ok = list.RemoveAll(b => b.Id == id) > 0;
        if (ok) Save(list);
        return ok;
    }

    private List<Book> Load()
    {
        if (!File.Exists(_path)) return new List<Book>();
        var json = File.ReadAllText(_path);
        if (string.IsNullOrWhiteSpace(json)) return new List<Book>();
        return JsonSerializer.Deserialize<List<Book>>(json, _opts) ?? new List<Book>();
    }

    private void Save(List<Book> list)
    {
        var json = JsonSerializer.Serialize(list, _opts);
        File.WriteAllText(_path, json);
    }
}
