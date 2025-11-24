using System;
using System.IO;
using System.Linq;
using Xunit;
using Fase06.RepositoryCsv;

public class CsvBookRepositoryTests
{
    private static string CreateTempPath()
    {
        var file = Path.GetTempFileName();
        File.WriteAllText(file, string.Empty);
        return file;
    }

    private static CsvBookRepository CreateRepo(string path)
        => new CsvBookRepository(path);

    [Fact]
    public void ListAll_WhenFileDoesNotExist_ShouldReturnEmpty()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv");
        var repo = CreateRepo(path);
        var all = repo.ListAll();
        Assert.Empty(all);
    }

    [Fact]
    public void Add_Then_ListAll_ShouldPersistInFile()
    {
        var path = CreateTempPath();
        var repo = CreateRepo(path);
        repo.Add(new Book(1, "Livro, com vírgula", "Autor \"com aspas\""));
        var all = repo.ListAll();
        Assert.Single(all);
        Assert.Equal(1, all[0].Id);
        Assert.Contains(",", all[0].Title);
        Assert.Contains("\"", all[0].Author);
    }

    [Fact]
    public void GetById_Existing_ShouldReturnBook()
    {
        var path = CreateTempPath();
        var repo = CreateRepo(path);
        repo.Add(new Book(1, "Livro A", "Autor"));
        var found = repo.GetById(1);
        Assert.NotNull(found);
        Assert.Equal("Livro A", found!.Title);
    }

    [Fact]
    public void GetById_Missing_ShouldReturnNull()
    {
        var path = CreateTempPath();
        var repo = CreateRepo(path);
        var found = repo.GetById(99);
        Assert.Null(found);
    }

    [Fact]
    public void Update_Existing_ShouldPersistChanges()
    {
        var path = CreateTempPath();
        var repo = CreateRepo(path);
        repo.Add(new Book(1, "Livro A", "Autor"));
        var updated = repo.Update(new Book(1, "Livro A (Revisto)", "Autor"));
        Assert.True(updated);
        Assert.Equal("Livro A (Revisto)", repo.GetById(1)!.Title);
    }

    [Fact]
    public void Remove_Existing_ShouldDeleteFromFile()
    {
        var path = CreateTempPath();
        var repo = CreateRepo(path);
        repo.Add(new Book(1, "Livro A", "Autor"));
        var removed = repo.Remove(1);
        Assert.True(removed);
        Assert.Empty(repo.ListAll());
    }
}
