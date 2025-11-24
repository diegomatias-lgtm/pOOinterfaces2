using Xunit;
using Fase08.ISP;

public class CatalogQueryTests
{
    [Fact]
    public void FindById_ReturnsBook_WhenExists()
    {
        var fake = new ReadOnlyFake();
        var query = new CatalogQuery(fake);
        var book = query.FindById(1);
        Assert.NotNull(book);
        Assert.Equal("DDD", book!.Title);
    }

    [Fact]
    public void FindById_ReturnsNull_WhenNotExists()
    {
        var fake = new ReadOnlyFake();
        var query = new CatalogQuery(fake);
        var book = query.FindById(99);
        Assert.Null(book);
    }

    [Fact]
    public void All_ReturnsAllBooks()
    {
        var fake = new ReadOnlyFake();
        var query = new CatalogQuery(fake);
        var all = query.All();
        Assert.Single(all);
        Assert.Equal(1, all[0].Id);
    }
}
