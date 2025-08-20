namespace ToDo.Application.Exceptions.ToDoItem;

public class PriorityIsInvalidException : Exception
{
    public PriorityIsInvalidException() : base("Priority can not have invalid value")
    {

    }

    public PriorityIsInvalidException(string message) : base(message)
    {

    }
}
