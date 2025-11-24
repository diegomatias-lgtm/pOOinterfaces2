using System;
using System.IO;
using System.Linq;
using Xunit;
using Fase07.RepositoryJson;

public class JsonBookRepositoryTests
{
    private static string CreateTempPath()
    {
        var file = Path.GetTempFileName();
        File.WriteAllText(file, string.Empty);
        return file;
    }

    private static JsonBookRepository CreateRepo(string path)
        => new JsonBookRepository(path);

    [Fact]
    public void ListAll_WhenFileDoesNotExist_ShouldReturnEmpty()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        var repo = CreateRepo(path);
        var all = repo.ListAll();
        Assert.Empty(all);
    }

    [Fact]
    public void Add_Then_ListAll_ShouldPersistInFile()
    {
        var path = CreateTempPath();
        var repo = CreateRepo(path);
        repo.Add(new Book(1, "Livro A", "Autor"));
        var all = repo.ListAll();
        Assert.Single(all);
        Assert.Equal(1, all[0].Id);
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
    public void Update_Missing_ShouldReturnFalse()
    {
        var path = CreateTempPath();
        var repo = CreateRepo(path);
        var updated = repo.Update(new Book(1, "Livro A", "Autor"));
        Assert.False(updated);
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

    [Fact]
    public void Remove_Missing_ShouldReturnFalse()
    {
        var path = CreateTempPath();
        var repo = CreateRepo(path);
        var removed = repo.Remove(99);
        Assert.False(removed);
    }

    [Fact]
    public void ListAll_WithEmptyFile_ShouldReturnEmpty()
    {
        var path = CreateTempPath();
        // arquivo já está vazio
        var repo = CreateRepo(path);
        var all = repo.ListAll();
        Assert.Empty(all);
    }

    [Fact]
    public void ListAll_WithWhitespaceFile_ShouldReturnEmpty()
    {
        var path = CreateTempPath();
        File.WriteAllText(path, "   \n\t");
        var repo = CreateRepo(path);
        var all = repo.ListAll();
        Assert.Empty(all);
    }
}
