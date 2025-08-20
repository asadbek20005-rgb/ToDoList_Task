namespace ToDo.Application.Exceptions.ToDoItem;

public class CreateModelNullException : Exception
{
    public CreateModelNullException() : base("Create model is null")
    {

    }

    public CreateModelNullException(string message) : base(message)
    {

    }

}
