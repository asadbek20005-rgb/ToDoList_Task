namespace ToDo.Application.Exceptions.ToDoItem;

public class StatusException : Exception
{
    public StatusException() : base("Status can not have invalid value")
    {

    }
    public StatusException(string message) : base(message)
    {

    }
}
