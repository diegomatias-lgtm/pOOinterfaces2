using System;
using System.Linq;
using PooInterface.Core.Models;
using PooInterface.Core.Repositories;
using Xunit;

namespace PooInterface.Tests
{
    public class RepositoryIspTests
    {
        [Fact]
        public void ReadOnlyConsumer_ShouldDependOnlyOnIReadRepository()
        {
            // Arrange
            var repo = new PooInterface.Core.Repositories.InMemoryRepository();
            var todo = new ToDo("test item");
            repo.Add(todo);

            // Act: use only the read interface
            IReadRepository<ToDo> readOnly = repo;
            var found = readOnly.GetById(todo.Id);

            // Assert
            Assert.NotNull(found);
            Assert.Equal("test item", found!.Title);
        }

        [Fact]
        public void CsvRepository_ReadsWrittenItem_PreservesId()
        {
            var path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid() + ".csv");
            try
            {
                var repo = new PooInterface.Core.Repositories.CsvRepository(path);
                var todo = new ToDo("csv item");
                repo.Add(todo);

                var read = repo.GetById(todo.Id);
                Assert.NotNull(read);
                Assert.Equal(todo.Id, read!.Id);
                Assert.Equal("csv item", read.Title);
            }
            finally
            {
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
        }
    }
}
