namespace ToDo.Application.Exceptions.ToDoItem;

public class CollectionCountIsZero : Exception
{
    public CollectionCountIsZero() : base("There is no data")
    {
        
    }
}
