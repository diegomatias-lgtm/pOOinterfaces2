using PooInterface.Core.Models;
using PooInterface.Core.Repositories;
using Xunit;

namespace PooInterface.Tests;

public class RepositoryTests
{
    [Fact]
    public void InMemoryRepository_AddAndGet_Works()
    {
        var repo = new InMemoryRepository();
        var t = new ToDo("task 1");
        repo.Add(t);
        var fetched = repo.GetById(t.Id);
        Assert.NotNull(fetched);
        Assert.Equal(t.Title, fetched!.Title);
    }

    [Fact]
    public void InMemoryRepository_Delete_Works()
    {
        var repo = new InMemoryRepository();
        var t = new ToDo("task 2");
        repo.Add(t);
        var ok = repo.Delete(t.Id);
        Assert.True(ok);
        Assert.Null(repo.GetById(t.Id));
    }
}