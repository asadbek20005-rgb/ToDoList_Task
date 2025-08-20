using ToDo.Application.Contracts.Services.Implementations;
using ToDo.Application.Models.ToDoItem;
using ToDo.Application.Validations;
using ToDo.Domain.Entities;
using ToDo.Domain.Enums;
using ToDo.Infrastructure.Persistence.Repositories;
var validator = new CreateModelValidation();
var repo = new BaseRepository<ToDoItem>();
var service = new ToDoItemService(repo, validator);

async Task ShowInterface()
{
    while (true)
    {
        Console.WriteLine("1. Add Task");
        Console.WriteLine("2. View All Tasks");
        Console.WriteLine("3. Delete Task");
        Console.WriteLine("4. View By Category");
        Console.WriteLine("5. Change Task Status");
        Console.WriteLine("6. Filter");
        Console.WriteLine("7. Sort");
        Console.WriteLine("8. Overdue Tasks");

        Console.Write("Select: ");


        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                await AddTask();
                break;
            case "2":
                ViewAllTasks();
                break;
            case "3":
                DeleteTask();
                break;
            case "4":
                ViewTasksByCategory();
                break;
            case "5":
                ChangeTaskStatus();
                break;
            case "6":
                Filter();
                break;
            case "7":
                Sort();
                break;
            case "8":
                GetOverdueTasks();
                break;
        }


    }


}

void GetOverdueTasks()
{
    var items = service.ViewOverdueTasks();
    foreach (var item in items)
    {
        Console.WriteLine($"Id: {item.Id}");
        Console.WriteLine($"Name: {item.Name}");
        Console.WriteLine($"Description: {item.Description}");
        Console.WriteLine($"Due date: {item.DueDate}");
        Console.WriteLine($"Category: {item.Category}");
        Console.WriteLine($"Status: {item.Status}");
        Console.WriteLine($"Description: {item.Priority}");
    }
    Console.ReadLine();
}

void Sort()
{
    while (true)
    {
        Console.WriteLine("You can sort by: ");
        Console.WriteLine("1. Due date");
        Console.WriteLine("2. Priority");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                GetByDueDate();
                break;
            case "2":
                GetByPriority();
                break;
        }

    }
}
void GetByPriority()
{
    while (true)
    {
        Console.WriteLine("Choose priority: ");
        Console.WriteLine("1. Low");
        Console.WriteLine("2. Medium");
        Console.WriteLine("3. High");

        var priority = (Priority)Enum.Parse(typeof(Priority), Console.ReadLine());

        var model = new SortModel
        {
            Priority = priority
        };
        var items = service.Sort(model);
        foreach (var item in items)
        {
            Console.WriteLine($"Id: {item.Id}");
            Console.WriteLine($"Name: {item.Name}");
            Console.WriteLine($"Description: {item.Description}");
            Console.WriteLine($"Due date: {item.DueDate}");
            Console.WriteLine($"Category: {item.Category}");
            Console.WriteLine($"Status: {item.Status}");
            Console.WriteLine($"Description: {item.Priority}");
        }
        break;
    }
    Console.ReadLine();
}
void GetByDueDate()
{
    while (true)
    {
        Console.WriteLine("Enter due date: ");
        var dueDate = DateTime.Parse(Console.ReadLine());
        var model = new SortModel
        {
            DueDate = dueDate,
        };
        var items = service.Sort(model);
        foreach (var item in items)
        {
            Console.WriteLine($"Id: {item.Id}");
            Console.WriteLine($"Name: {item.Name}");
            Console.WriteLine($"Description: {item.Description}");
            Console.WriteLine($"Due date: {item.DueDate}");
            Console.WriteLine($"Category: {item.Category}");
            Console.WriteLine($"Status: {item.Status}");
            Console.WriteLine($"Description: {item.Priority}");
        }
        break;
    }
    Console.ReadLine();
}
void Filter()
{
    while (true)
    {
        Console.WriteLine("you can filter by:");
        Console.WriteLine("1. Status");
        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                GetByStatus();
                break;
        }
    }
}
void GetByStatus()
{
    while (true)
    {
        Console.WriteLine("Choose status: ");
        Console.WriteLine("1. Not started");
        Console.WriteLine("2. In progress");
        Console.WriteLine("3. Completed");
        var status = (Status)Enum.Parse(typeof(Status), Console.ReadLine());
        var model = new FilterModel
        {
            Status = status
        };
        var items = service.Filter(model);
        foreach (var item in items)
        {
            Console.WriteLine($"Id: {item.Id}");
            Console.WriteLine($"Name: {item.Name}");
            Console.WriteLine($"Description: {item.Description}");
            Console.WriteLine($"Due date: {item.DueDate}");
            Console.WriteLine($"Category: {item.Category}");
            Console.WriteLine($"Status: {item.Status}");
            Console.WriteLine($"Description: {item.Priority}");
        }
        break;
    }
    Console.ReadLine();
}
void ChangeTaskStatus()
{
    while (true)
    {
        Console.WriteLine("Enter task id: ");
        var id = long.Parse(Console.ReadLine());
        Console.WriteLine("Choose status: ");
        Console.WriteLine("1. Not started");
        Console.WriteLine("2. In progress");
        Console.WriteLine("3. Completed");
        var status = (Status)Enum.Parse(typeof(Status), Console.ReadLine());
        service.ChangeTaskStatus(id, status);
        break;
    }
}
void ViewTasksByCategory()
{
    while (true)
    {
        Console.WriteLine("Choose category: ");
        Console.WriteLine("1. Work");
        Console.WriteLine("2. Personal");
        Console.WriteLine("3. Study");

        var category = (Category)Enum.Parse(typeof(Category), Console.ReadLine());
        var items = service.ViewTasksByCategory(category);
        foreach (var item in items)
        {
            Console.WriteLine($"Id: {item.Id}");
            Console.WriteLine($"Name: {item.Name}");
            Console.WriteLine($"Description: {item.Description}");
            Console.WriteLine($"Due date: {item.DueDate}");
            Console.WriteLine($"Category: {item.Category}");
            Console.WriteLine($"Status: {item.Status}");
            Console.WriteLine($"Description: {item.Priority}");
        }
        break;
    }
    Console.ReadLine();
}
void DeleteTask()
{
    while (true)
    {
        Console.WriteLine("Enter task id: ");
        var id = long.Parse(Console.ReadLine());
        service.DeleteItem(id);
        break;
    }
}
void ViewAllTasks()
{
    var items = service.ViewAllTasks();
    foreach (var item in items)
    {
        Console.WriteLine($"Id: {item.Id}");
        Console.WriteLine($"Name: {item.Name}");
        Console.WriteLine($"Description: {item.Description}");
        Console.WriteLine($"Due date: {item.DueDate}");
        Console.WriteLine($"Category: {item.Category}");
        Console.WriteLine($"Status: {item.Status}");
        Console.WriteLine($"Description: {item.Priority}");
    }
    Console.ReadLine();
}
async Task AddTask()
{

    while (true)
    {
        Console.WriteLine("Enter task name: ");
        var name = Console.ReadLine();
        Console.WriteLine("Enter description: ");
        var des = Console.ReadLine();
        Console.WriteLine("Choose category: " +
            "1. Work" +
            "2. Personal" +
            "3. Study");
        var category = (Category)Enum.Parse(typeof(Category), Console.ReadLine());
        Console.WriteLine("Choose priority: " +
            "1. Low" +
            "2. Medium" +
            "3. High");
        var priority = (Priority)Enum.Parse(typeof(Priority), Console.ReadLine());
        Console.WriteLine("Enter due date name: ");
        var dueDate = DateTime.Parse(Console.ReadLine());
        Console.WriteLine("Choose status: " +
            "1. Not started" +
            "2. In progress" +
            "3. Completed");
        var status = (Status)Enum.Parse(typeof(Status), Console.ReadLine());

        var model = new CreateModel
        {
            Name = name,
            Category = category,
            Description = des,
            Priority = priority,
            DueDate = dueDate,
            Status = status
        };

        await service.CreateItem(model);
        break;
    }


}
await ShowInterface();