namespace School.Domain.Entities;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int WorkloadHours { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Enrollment> Enrollments { get; } = new();
}
