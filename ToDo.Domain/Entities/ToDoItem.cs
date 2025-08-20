using ToDo.Domain.Enums;

namespace ToDo.Domain.Entities;

public class ToDoItem
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required Priority Priority { get; set; }
    public DateTime DueDate { get; set; }
    public required Category Category { get; set; }
    public required Status Status { get; set; }

}
