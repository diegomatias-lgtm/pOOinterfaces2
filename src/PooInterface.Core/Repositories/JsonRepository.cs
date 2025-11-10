using System.Text.Json;
using PooInterface.Core.Models;

namespace PooInterface.Core.Repositories;

// Fase 9 — JSON repository (simple)
public class JsonRepository : IRepository<ToDo>
{
    private readonly string _filePath;
    private readonly object _lock = new();

    public JsonRepository(string filePath)
    {
        _filePath = filePath;
        if (!File.Exists(_filePath))
            File.WriteAllText(_filePath, "[]");
    }

    public void Add(ToDo item)
    {
        lock (_lock)
        {
            var all = ReadAll().ToList();
            all.Add(item);
            Write(all);
        }
    }

    public bool Delete(Guid id)
    {
        lock (_lock)
        {
            var all = ReadAll().ToList();
            var removed = all.RemoveAll(x => x.Id == id);
            if (removed == 0) return false;
            Write(all);
            return true;
        }
    }

    public ToDo? GetById(Guid id) => ReadAll().FirstOrDefault(x => x.Id == id);

    public IEnumerable<ToDo> List() => ReadAll();

    public void Update(ToDo item)
    {
        lock (_lock)
        {
            var all = ReadAll().ToList();
            var idx = all.FindIndex(x => x.Id == item.Id);
            if (idx >= 0) all[idx] = item;
            else all.Add(item);
            Write(all);
        }
    }

    private IEnumerable<ToDo> ReadAll()
    {
        var json = File.ReadAllText(_filePath);
        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        try
        {
            return JsonSerializer.Deserialize<List<ToDo>>(json, opts) ?? Enumerable.Empty<ToDo>();
        }
        catch
        {
            return Enumerable.Empty<ToDo>();
        }
    }

    private void Write(IEnumerable<ToDo> items)
    {
        var opts = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(items, opts);
        File.WriteAllText(_filePath, json);
    }
}