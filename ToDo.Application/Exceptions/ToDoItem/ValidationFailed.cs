namespace ToDo.Application.Exceptions.ToDoItem;

public class ValidationFailed : Exception
{
    public ValidationFailed(string message) : base(message)
    {
        
    }
}
