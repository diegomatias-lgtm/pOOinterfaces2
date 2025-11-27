using System.Globalization;
using System.Text;
using PooInterface.Core.Models;

namespace PooInterface.Core.Repositories;

// Simple CSV repository. Not production hardened — for educational purposes (Fase 8)
public class CsvRepository : IRepository<ToDo>
{
    private readonly string _filePath;
    private readonly object _lock = new();

    public CsvRepository(string filePath)
    {
        _filePath = filePath;
        EnsureFile();
    }

    private void EnsureFile()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "Id,Title,Done\n", Encoding.UTF8);
        }
    }

    public void Add(ToDo item)
    {
        lock (_lock)
        {
            var line = Escape(item.Id.ToString()) + "," + Escape(item.Title) + "," + item.Done.ToString(CultureInfo.InvariantCulture);
            File.AppendAllText(_filePath, line + "\n", Encoding.UTF8);
        }
    }

    public bool Delete(Guid id)
    {
        lock (_lock)
        {
            var all = ReadAll().ToList();
            var removed = all.RemoveAll(x => x.Id == id);
            if (removed == 0) return false;
            Overwrite(all);
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
            Overwrite(all);
        }
    }

    private List<ToDo> ReadAll()
    {
        var lines = File.ReadAllLines(_filePath, Encoding.UTF8).Skip(1);
        var list = new List<ToDo>();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = SplitCsv(line);
            if (parts.Length < 3) continue;
            if (!Guid.TryParse(parts[0], out var id)) continue;
            var title = Unescape(parts[1]);
            var done = bool.TryParse(parts[2], out var d) && d;
            // use object initializer to set init-only Id safely (avoids reflection)
            var item = new ToDo(title) { Id = id, Done = done };
            list.Add(item);
        }
        return list;
    }

    private void Overwrite(List<ToDo> all)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id,Title,Done");
        foreach (var it in all)
        {
            var line = Escape(it.Id.ToString()) + "," + Escape(it.Title) + "," + it.Done.ToString(CultureInfo.InvariantCulture);
            sb.AppendLine(line);
        }
        File.WriteAllText(_filePath, sb.ToString(), Encoding.UTF8);
    }

    private static string Escape(string s) => s.Replace("\"", "\"\"");
    private static string Unescape(string s) => s;

    private static string[] SplitCsv(string line)
    {
        // naive split — ok for this educational example
        return line.Split(',');
    }
}