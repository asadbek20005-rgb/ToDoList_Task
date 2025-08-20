namespace ToDo.Application.Exceptions.ToDoItem;

public class CategoryException : Exception
{
    public CategoryException() : base("Category can not have invalid value")
    {
        
    }
    public CategoryException(string message) : base(message)
    {
        
    }
}
