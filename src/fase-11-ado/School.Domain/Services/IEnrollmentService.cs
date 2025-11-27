using System;
using System.Collections.Generic;
using School.Domain.Entities;

namespace School.Domain.Services;

public interface IEnrollmentService
{
    Enrollment EnrollStudentInCourse(int studentId, int courseId, DateTime enrollmentDate);
    void CancelEnrollment(int enrollmentId);
    IReadOnlyList<Enrollment> GetEnrollmentsByStudent(int studentId);
    IReadOnlyList<Enrollment> GetEnrollmentsByCourse(int courseId);
}
