namespace ToDo.Application.Exceptions.ToDoItem;

public class IdIsNotCorrectFromatException : Exception
{
    public IdIsNotCorrectFromatException(string message) : base(message)
    {
        
    }
}
