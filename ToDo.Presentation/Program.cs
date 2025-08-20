using ToDo.Application.Contracts.Services.Implementations;
using ToDo.Application.Dtos;
using ToDo.Application.Exceptions.ToDoItem;
using ToDo.Application.Models.ToDoItem;
using ToDo.Application.Validations;
using ToDo.Domain.Entities;
using ToDo.Domain.Enums;
using ToDo.Infrastructure.Persistence.Repositories;
var createModelValidator = new CreateModelValidation();
var changeTaskStatusModelValidator = new ChangeTaskStatusModelValidator();
var filterModelValidator = new FilterModelValidator();
var sortModelValidator = new SortModelValidator();
var baseRepository = new BaseRepository<ToDoItem>();
var service = new ToDoItemService(baseRepository,
    createModelValidator,
    changeTaskStatusModelValidator,
    filterModelValidator,
    sortModelValidator
    );

async Task ShowInterface()
{
    while (true)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Hi there! By using this project, you can manage your plans more organized and reliable. The following actions and commands are required to use the project:\r\nEXIT - goes back. For example:\r\nInput: exit or EXIT\r\n\r\nSelect the desired action by selecting the list numbers. For example:\r\n1. Get\r\n2. Add\r\n3. Delete\r\nInput: 1\r\nThis will execute the get action\r\nGood luck :)");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("1. Add Task");
        Console.WriteLine("2. View All Tasks");
        Console.WriteLine("3. Delete Task");
        Console.WriteLine("4. View By Category");
        Console.WriteLine("5. Change Task Status");
        Console.WriteLine("6. Filter");
        Console.WriteLine("7. Sort");
        Console.WriteLine("8. Overdue Tasks");
        Console.ResetColor();

        Console.Write("Select: ");


        var choice = Console.ReadLine();
        if (choice.ToLower().Equals("exit"))
        {
            break;
        }

        switch (choice)
        {
            case "1":
                await AddTask();
                break;
            case "2":
                ViewAllTasks();
                break;
            case "3":
                await DeleteTask();
                break;
            case "4":
                await ViewTasksByCategory();
                break;
            case "5":
                await ChangeTaskStatus();
                break;
            case "6":
                await Filter();
                break;
            case "7":
                await Sort();
                break;
            case "8":
                GetOverdueTasks();
                break;
        }
    }
}
void GetOverdueTasks()
{
    try
    {

        var items = service.ViewOverdueTasks();
        ShowItems(items);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Press ENTER to back");
        Console.ResetColor();

        Console.ReadLine();
    }
    catch (CollectionCountIsZero e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: " + e.Message);
        Console.ResetColor();
    }
    catch (Exception e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: " + e.Message);
        Console.ResetColor();
    }
}
async Task Sort()
{
    while (true)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("1. Due date");
        Console.WriteLine("2. Priority");
        Console.ResetColor();
        Console.Write("You can sort by: ");

        var choice = Console.ReadLine();
        if (choice.ToLower().Equals("exit"))
        {
            await Back();
        }

        switch (choice)
        {
            case "1":
                await GetByDueDate();
                break;
            case "2":
                await GetByPriority();
                break;

        }

    }
}
async Task GetByPriority()
{
    while (true)
    {
        try
        {
            Priority priority = await ChoosePriorityAsync();
            var model = new SortModel
            {
                Priority = priority
            };
            var items = await service.Sort(model);
            ShowItems(items);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Press ENTER to back");
            Console.ResetColor();

            Console.ReadLine();
            break;


        }
        catch (ValidationFailed e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        catch (PriorityIsInvalidException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }

    }
}
async Task GetByDueDate()
{
    while (true)
    {
        try
        {
            Console.Write("Enter due date name: ");
            var dueDateValue = Console.ReadLine();
            if (dueDateValue.ToLower().Equals("exit"))
            {
                await Back();
            }
            DateTime parsedDateTime;
            if (DateTime.TryParse(dueDateValue, out parsedDateTime))
            {
                var model = new SortModel
                {
                    DueDate = parsedDateTime,
                };
                var items = await service.Sort(model);
                ShowItems(items);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Press ENTER to back");
                Console.ResetColor();
                Console.ReadLine();
                break;
            }
            else
            {
                throw new DueDateException("Invalid due date");
            }

        }
        catch (ValidationFailed e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        catch (DueDateException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }

    }
}
async Task Filter()
{
    while (true)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("1. Status");
        Console.ResetColor();
        Console.Write("you can filter by: ");
        var choice = Console.ReadLine();
        if (choice.ToLower().Equals("exit"))
        {
            await Back();
        }
        switch (choice)
        {
            case "1":
                await GetByStatus();
                break;
        }
    }
}
async Task GetByStatus()
{
    while (true)
    {

        try
        {

            Status status = await ChooseStatusAsync();
            var model = new FilterModel
            {
                Status = status
            };
            var items = await service.Filter(model);
            ShowItems(items);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Press ENTER to back");
            Console.ResetColor();

            Console.ReadLine();
            break;


        }
        catch (ExistException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        catch (CollectionCountIsZero e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        catch (ValidationFailed e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        catch (StatusException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
    }
}
async Task ChangeTaskStatus()
{
    while (true)
    {
        try
        {
            Console.Write("Enter task id: ");
            var idValue = Console.ReadLine();
            if (idValue.ToLower().Equals("exit"))
            {
                await Back();
            }
            if (long.TryParse(idValue, out var id))
            {
                Status status = await ChooseStatusAsync();
                var model = new ChangeTaskStatusModel
                {
                    Id = id,
                    Status = status
                };
                await service.ChangeTaskStatus(model);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Status succesfully CHANGED");
                Console.Write("Press ENTER to back");
                Console.ResetColor();
                Console.ReadLine();
                break;



            }
            else
            {
                throw new IdIsNotCorrectFromatException("Invalid Id");
            }

        }
        catch (ExistException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        catch (ValidationFailed e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();

        }
        catch (IdIsNotCorrectFromatException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();

        }
        catch (StatusException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();

        }

    }
}
async Task ViewTasksByCategory()
{
    try
    {
        while (true)
        {

            Category category = await ChooseCategoryAsync();
            var items = service.ViewTasksByCategory(category);
            ShowItems(items);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Press ENTER to back");
            Console.ResetColor();

            Console.ReadLine();


        }
    }
    catch (CategoryException e)
    {

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: " + e.Message);
        Console.ResetColor();
    }
    catch (CollectionCountIsZero e)
    {

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: " + e.Message);
        Console.ResetColor();
    }
    catch (Exception e)
    {

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: " + e.Message);
        Console.ResetColor();
    }
}
async Task DeleteTask()
{
    while (true)
    {

        try
        {
            Console.Write("Enter task id: ");
            var idValue = Console.ReadLine();
            if (idValue.ToLower().Equals("exit"))
            {
                await Back();
            }
            if (long.TryParse(idValue, out var id))
            {
                service.DeleteItem(id);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Task is succecfully DELETED");
                Console.Write("Press ENTER to back");
                Console.ResetColor();

                Console.ReadLine();
                break;
            }
            else
            {
                throw new IdIsNotCorrectFromatException("Invalid Id");
            }
        }
        catch (ExistException e)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();

        }
        catch (IdIsNotCorrectFromatException e)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
    }
}
void ViewAllTasks()
{
    try
    {
        var items = service.ViewAllTasks();
        ShowItems(items);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Press ENTER to back");
        Console.ResetColor();

        Console.ReadLine();
    }
    catch (CollectionCountIsZero e)
    {

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: " + e.Message);
        Console.ResetColor();

    }
}
async Task AddTask()
{

    while (true)
    {

        try
        {
            var model = new CreateModel();
            Console.Write("Enter task name: ");
            var name = Console.ReadLine();

            if (name.ToLower().Equals("exit"))
            {
                await Back();
            }
            model.Name = name;
            Console.Write("Enter description: ");
            var des = Console.ReadLine();
            if (des.ToLower().Equals("exit"))
            {
                await Back();
            }
            model.Description = des;

            Console.Write("Enter due date name: ");
            var dueDateValue = Console.ReadLine();
            if (dueDateValue.ToLower().Equals("exit"))
            {
                await Back();
            }
            DateTime parsedDateTime;
            if (DateTime.TryParse(dueDateValue, out parsedDateTime))
            {
                model.DueDate = parsedDateTime;
            }
            else
            {
                throw new DueDateException("Invalid due date");
            }

            Category category = await ChooseCategoryAsync();
            model.Category = category;
            Priority priority = await ChoosePriorityAsync();
            model.Priority = priority;
            Status status = await ChooseStatusAsync();
            model.Status = status;

            await service.CreateItem(model);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Task is succecfully ADDED");
            Console.Write("Press ENTER to back");
            Console.ResetColor();

            Console.ReadLine();
            break;
        }
        catch (ValidationFailed e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        catch (CategoryException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        catch (PriorityIsInvalidException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        catch (DueDateException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        catch (StatusException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
    }
}
void ShowItems(List<ToDoItemDto> items)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Result:");
    Console.ResetColor();
    Console.WriteLine("[");

    foreach (var item in items)
    {
        Console.WriteLine("  {");
        Console.WriteLine($"    Id          : {item.Id}");
        Console.WriteLine($"    Name        : {item.Name}");
        Console.WriteLine($"    Description : {item.Description}");
        Console.WriteLine($"    Due date    : {item.DueDate:yyyy-MM-dd}");
        Console.WriteLine($"    Category    : {item.Category}");
        Console.WriteLine($"    Status      : {item.Status}");
        Console.WriteLine($"    Priority    : {item.Priority}");
        Console.WriteLine("  },");
    }

    Console.WriteLine("]");
}
async Task<Category> ChooseCategoryAsync()
{
    Console.WriteLine("1. Work");
    Console.WriteLine("2. Personal");
    Console.WriteLine("3. Study");
    Console.ResetColor();

    Console.Write("Choose category: ");

    var value = Console.ReadLine();
    if (value.ToLower().Equals("exit"))
    {
        await Back();
    }
    Category category;
    if (Enum.TryParse(value, out category))
    {
        return category;
    }
    else
    {
        throw new CategoryException();
    }
}
async Task<Priority> ChoosePriorityAsync()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("1. Low");
    Console.WriteLine("2. Medium");
    Console.WriteLine("3. High");
    Console.ResetColor();

    Console.Write("Choose priority: ");

    var value2 = Console.ReadLine();
    if (value2.ToLower().Equals("exit"))
    {
        await Back();
    }
    Priority priority;
    if (Enum.TryParse(value2, out priority))
    {
        return priority;
    }
    else
    {
        throw new PriorityIsInvalidException();
    }
}
async Task<Status> ChooseStatusAsync()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("1. Not started");
    Console.WriteLine("2. In progress");
    Console.WriteLine("3. Completed");
    Console.ResetColor();

    Console.Write("Choose status: ");

    var statusValue = Console.ReadLine();
    if (statusValue.ToLower().Equals("exit"))
    {
        await Back();
    }
    Status status;

    if (Enum.TryParse(statusValue, out status))
    {
        return status;
    }
    else
    {
        throw new StatusException();
    }
}
async Task Back()
{
    await ShowInterface();
}
await ShowInterface();