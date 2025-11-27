using System;
using System.Collections.Generic;
using School.Domain.Entities;
using School.Domain.Repositories;

namespace School.Domain.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepo;
    private readonly IStudentRepository _studentRepo;
    private readonly ICourseRepository _courseRepo;

    public EnrollmentService(IEnrollmentRepository enrollmentRepo, IStudentRepository studentRepo, ICourseRepository courseRepo)
    {
        _enrollmentRepo = enrollmentRepo;
        _studentRepo = studentRepo;
        _courseRepo = courseRepo;
    }

    public Enrollment EnrollStudentInCourse(int studentId, int courseId, DateTime enrollmentDate)
    {
        var student = _studentRepo.GetById(studentId) ?? throw new InvalidOperationException("Student not found");
        var course = _courseRepo.GetById(courseId) ?? throw new InvalidOperationException("Course not found");
        if (!course.IsActive) throw new InvalidOperationException("Course is not active");

        var exists = _enrollmentRepo.Find(e => e.StudentId == studentId && e.CourseId == courseId && e.Status == "Active");
        if (exists.Count > 0) throw new InvalidOperationException("Student already enrolled in this course");

        var enrollment = new Enrollment { StudentId = studentId, CourseId = courseId, EnrollmentDate = enrollmentDate, Status = "Active" };
        _enrollmentRepo.Add(enrollment);
        return enrollment;
    }

    public void CancelEnrollment(int enrollmentId)
    {
        var e = _enrollmentRepo.GetById(enrollmentId);
        if (e is null) return;
        e.Status = "Cancelled";
        _enrollmentRepo.Update(e);
    }

    public IReadOnlyList<Enrollment> GetEnrollmentsByStudent(int studentId) => _enrollmentRepo.ListByStudent(studentId);

    public IReadOnlyList<Enrollment> GetEnrollmentsByCourse(int courseId) => _enrollmentRepo.ListByCourse(courseId);
}
