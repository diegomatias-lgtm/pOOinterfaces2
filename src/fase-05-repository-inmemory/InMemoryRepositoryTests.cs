using System.Collections.Generic;
using Xunit;
using Fase05.RepositoryInMemory;

public class InMemoryRepositoryTests
{
    private static InMemoryRepository<Book, int> CreateRepo()
        => new InMemoryRepository<Book, int>(b => b.Id);

    [Fact]
    public void Add_Then_ListAll_ShouldReturnOneItem()
    {
        var repo = CreateRepo();
        repo.Add(new Book(1, "Livro A", "Autor"));
        var all = repo.ListAll();
        Assert.Single(all);
        Assert.Equal(1, all[0].Id);
    }

    [Fact]
    public void GetById_Existing_ShouldReturnEntity()
    {
        var repo = CreateRepo();
        repo.Add(new Book(1, "Livro A", "Autor"));
        var found = repo.GetById(1);
        Assert.NotNull(found);
        Assert.Equal("Livro A", found!.Title);
    }

    [Fact]
    public void GetById_Missing_ShouldReturnNull()
    {
        var repo = CreateRepo();
        var found = repo.GetById(99);
        Assert.Null(found);
    }

    [Fact]
    public void Update_Existing_ShouldReturnTrue()
    {
        var repo = CreateRepo();
        repo.Add(new Book(1, "Livro A", "Autor"));
        var updated = repo.Update(new Book(1, "Livro A (Revisto)", "Autor"));
        Assert.True(updated);
        Assert.Equal("Livro A (Revisto)", repo.GetById(1)!.Title);
    }

    [Fact]
    public void Update_Missing_ShouldReturnFalse()
    {
        var repo = CreateRepo();
        var updated = repo.Update(new Book(1, "Livro A", "Autor"));
        Assert.False(updated);
    }

    [Fact]
    public void Remove_Existing_ShouldReturnTrue()
    {
        var repo = CreateRepo();
        repo.Add(new Book(1, "Livro A", "Autor"));
        var removed = repo.Remove(1);
        Assert.True(removed);
        Assert.Empty(repo.ListAll());
    }

    [Fact]
    public void Remove_Missing_ShouldReturnFalse()
    {
        var repo = CreateRepo();
        var removed = repo.Remove(99);
        Assert.False(removed);
    }
}
