using ToDo.Domain.Enums;

namespace ToDo.Application.Models.ToDoItem;

public class ChangeTaskStatusModel
{
    public long Id { get; set; }
    public Status Status { get; set; }
}
