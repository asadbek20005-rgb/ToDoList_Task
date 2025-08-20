namespace ToDo.Application.Exceptions.ToDoItem;

public class DueDateException : Exception
{
    public DueDateException() : base("Due date must be high or equal than present period")
    {

    }

    public DueDateException(string message) : base(message)
    {

    }
}
