using ToDo.Domain.Enums;

namespace ToDo.Application.Dtos;

public class ToDoItemDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Priority Priority { get; set; }
    public DateTime DueDate { get; set; }
    public Category Category { get; set; }
    public Status Status { get; set; }
}

