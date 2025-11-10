namespace PooInterface.Core.Models;

public sealed class ToDo
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public bool Done { get; set; }

    public ToDo(string title)
    {
        Id = Guid.NewGuid();
        Title = title ?? string.Empty;
        Done = false;
    }
}