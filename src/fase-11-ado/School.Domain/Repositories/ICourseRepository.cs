using School.Domain.Entities;

namespace School.Domain.Repositories;

public interface ICourseRepository : IReadRepository<Course, int>, IWriteRepository<Course, int>
{
}
