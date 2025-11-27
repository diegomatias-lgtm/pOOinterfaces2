using System;
using Microsoft.Extensions.Configuration;
using School.Persistence.AdoNet.Sqlite.Connections;
using School.Persistence.AdoNet.Sqlite.Repositories;
using School.Domain.Repositories;
using School.Domain.Services;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration.GetConnectionString("Default") ?? "Data Source=school.db";
var connectionFactory = new SqliteConnectionFactory(connectionString);
ICourseRepository courseRepository = new SqliteCourseRepository(connectionFactory);

Console.WriteLine("School Console - Courses (demo)");
while (true)
{
    Console.WriteLine();
    Console.WriteLine("1 - List courses");
    Console.WriteLine("2 - Add course");
    Console.WriteLine("3 - Update course");
    Console.WriteLine("4 - Remove course");
    Console.WriteLine("0 - Exit");
    Console.Write("Option: ");
    var opt = Console.ReadLine();
    if (opt == "0") break;
    switch (opt)
    {
        case "1":
            var list = courseRepository.ListAll();
            foreach (var c in list) Console.WriteLine($"{c.Id} - {c.Name} ({c.WorkloadHours}h) Active:{c.IsActive}");
            break;
        case "2":
            Console.Write("Name: "); var name = Console.ReadLine() ?? string.Empty;
            Console.Write("Workload hours: "); int.TryParse(Console.ReadLine(), out var hours);
            var course = new School.Domain.Entities.Course { Name = name, WorkloadHours = hours, IsActive = true };
            courseRepository.Add(course);
            Console.WriteLine($"Added with id {course.Id}");
            break;
        case "3":
            Console.Write("Id: "); int.TryParse(Console.ReadLine(), out var id);
            var found = courseRepository.GetById(id);
            if (found is null) { Console.WriteLine("Not found"); break; }
            Console.Write($"Name ({found.Name}): "); var n = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(n)) found.Name = n;
            Console.Write($"Workload ({found.WorkloadHours}): "); if (int.TryParse(Console.ReadLine(), out var w)) found.WorkloadHours = w;
            courseRepository.Update(found);
            Console.WriteLine("Updated.");
            break;
        case "4":
            Console.Write("Id: "); int.TryParse(Console.ReadLine(), out var rid);
            courseRepository.Remove(rid);
            Console.WriteLine("Removed (if existed).");
            break;
        default:
            Console.WriteLine("Invalid option");
            break;
    }
}
