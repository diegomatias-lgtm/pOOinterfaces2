using School.Domain.Entities;
using School.Persistence.AdoNet.InMemory;
using Xunit;

namespace School.Tests
{
    public class CourseTests
    {
        [Fact]
        public void InMemory_Add_Get_List_Update_Remove()
        {
            var repo = new InMemoryRepository<Course, int>(c => c.Id);
            var course = new Course { Id = 1, Name = "Math", WorkloadHours = 40, IsActive = true };
            repo.Add(course);
            var f = repo.GetById(1);
            Assert.NotNull(f);
            Assert.Equal("Math", f!.Name);
            var all = repo.ListAll();
            Assert.Single(all);
            course.Name = "Mathematics";
            repo.Update(course);
            Assert.Equal("Mathematics", repo.GetById(1)!.Name);
            repo.Remove(1);
            Assert.Null(repo.GetById(1));
        }
    }
}
