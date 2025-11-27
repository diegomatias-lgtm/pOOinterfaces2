using School.Domain.Entities;

namespace School.Domain.Repositories;

public interface IStudentRepository : IReadRepository<Student, int>, IWriteRepository<Student, int>
{
}
