namespace ToDo.Application.Exceptions.ToDoItem;

public class ExistException : Exception
{
    public ExistException(string message) : base(message)
    {
        
    }
 
}
