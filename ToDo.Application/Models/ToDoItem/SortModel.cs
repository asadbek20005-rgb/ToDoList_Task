using ToDo.Domain.Enums;

namespace ToDo.Application.Models.ToDoItem;

public class SortModel
{
    public DateTime? DueDate { get; set; }
    public Priority? Priority { get; set; }
}
