using School.Domain.Entities;
using System.Collections.Generic;

namespace School.Domain.Repositories;

public interface IEnrollmentRepository : IReadRepository<Enrollment, int>, IWriteRepository<Enrollment, int>
{
    IReadOnlyList<Enrollment> ListByStudent(int studentId);
    IReadOnlyList<Enrollment> ListByCourse(int courseId);
}
