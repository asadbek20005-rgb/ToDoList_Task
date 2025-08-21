using FluentValidation;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.Dtos;
using ToDo.Application.Exceptions.ToDoItem;
using ToDo.Application.Models.ToDoItem;
using ToDo.Domain.Entities;
using ToDo.Domain.Enums;

namespace ToDo.Application.Contracts.Services.Implementations;

public class ToDoItemService(
    IBaseRepository<ToDoItem> toItemRepository,
    IValidator<CreateModel> createValidation,
    IValidator<ChangeTaskStatusModel> changeTaskModelValidator,
    IValidator<FilterModel> filterModelValidator,
    IValidator<SortModel> sortModelValidator) : IToDoItemService
{
    private readonly IBaseRepository<ToDoItem> _toDoItemRepository = toItemRepository;
    private readonly IValidator<CreateModel> _createValidaton = createValidation;
    private readonly IValidator<ChangeTaskStatusModel> _changeTaskModelvalidator = changeTaskModelValidator;
    private readonly IValidator<FilterModel> _filterModelValidator = filterModelValidator;
    private readonly IValidator<SortModel> _sortModelValidator = sortModelValidator;
    public async Task ChangeTaskStatus(ChangeTaskStatusModel model)
    {
        var validationResult = await _changeTaskModelvalidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var err in validationResult.Errors)
            {
                throw new ValidationFailed(err.ErrorMessage);
            }
        }
        var item = _toDoItemRepository.ViewAllValues().Where(x => x.Id == model.Id).FirstOrDefault();
        if (item is null) throw new ExistException($"Item with id: {model.Id} is not exist");
        item.Status = model.Status;
        _toDoItemRepository.Save();
    }
    public async Task CreateItem(CreateModel model)
    {
        try
        {
            var validatonResult = await _createValidaton.ValidateAsync(model);
            if (!validatonResult.IsValid)
            {
                foreach (var err in validatonResult.Errors)
                {
                    throw new ValidationFailed(err.ErrorMessage);
                }
            }
            CheckToDoItemExist(model.Name);


            var newItem = new ToDoItem()
            {
                Id = new Random().Next(1, 99),
                Name = model.Name,
                Category = model.Category,
                Status = model.Status,
                Description = model.Description,
                Priority = model.Priority,
                DueDate = model.DueDate,
            };


            _toDoItemRepository.AddTask(newItem);
            _toDoItemRepository.Save();
        }
        catch (ExistException e)
        {
            Console.WriteLine(e.Message);
        }
    }
    public void CheckToDoItemExist(string name)
    {
        var exist = _toDoItemRepository.ViewAllValues().Any(x => x.Name == name);
        if (exist) throw new ExistException($"To do item with {name} is already exist");
    }
    public void DeleteItem(long id)
    {
        if (id < 0) throw new IdIsNotCorrectFromatException($"Id: {id} must be greater than 0");
        var item = _toDoItemRepository.ViewAllValues().Where(x => x.Id == id).FirstOrDefault();
        if (item is null) throw new ExistException($"Item with id: {id} is not exist");
        _toDoItemRepository.DeleteTask(item);
        _toDoItemRepository.Save();


    }
    public async Task<List<ToDoItemDto>> Filter(FilterModel sortModel)
    {
        var validationResult = await _filterModelValidator.ValidateAsync(sortModel);
        if (!validationResult.IsValid)
        {
            foreach (var err in validationResult.Errors)
            {
                throw new ValidationFailed(err.ErrorMessage);
            }
        }

        var items = _toDoItemRepository.ViewAllValues().Where(x => x.Status == sortModel.Status).ToList();
        if (items.Count == 0) throw new CollectionCountIsZero();
        return ConvertToDto(items);
    }
    public async Task<List<ToDoItemDto>> Sort(SortModel sortModel)
    {
        try
        {
            var validatonResult = await _sortModelValidator.ValidateAsync(sortModel);
            if (!validatonResult.IsValid)
            {
                foreach (var err in validatonResult.Errors)
                {
                    throw new ValidationFailed(err.ErrorMessage);
                }
            }
            if (sortModel.DueDate is not null)
                return GetByTasksByDueDate(sortModel.DueDate);
            if (sortModel.Priority is not null)
                return GetTasksByPriority(sortModel.Priority);

        }
        catch (CollectionCountIsZero e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + e.Message);
            Console.ResetColor();
        }
        return new List<ToDoItemDto>();
    }
    public List<ToDoItemDto> GetTasksByPriority(Priority? priority)
    {
        var items = _toDoItemRepository.ViewAllValues().Where(x => x.Priority == priority).ToList();
        if (items.Count == 0) throw new CollectionCountIsZero();
        return ConvertToDto(items);
    }
    public List<ToDoItemDto> GetByTasksByDueDate(DateTime? dueDate)
    {
        var items = _toDoItemRepository.ViewAllValues().Where(x => x.DueDate <= dueDate).ToList();
        if (items.Count == 0) throw new CollectionCountIsZero();
        return ConvertToDto(items);
    }
    public List<ToDoItemDto> ViewAllTasks()
    {
        var items = _toDoItemRepository.ViewAllValues().ToList();
        if (items.Count == 0) throw new CollectionCountIsZero();
        return ConvertToDto(items);
    }
    public List<ToDoItemDto> ViewOverdueTasks()
    {
        var items = _toDoItemRepository.ViewAllValues().Where(x => x.DueDate < DateTime.Now).ToList();
        if (items.Count == 0) throw new CollectionCountIsZero();
        return ConvertToDto(items);
    }
    public List<ToDoItemDto> ViewTasksByCategory(Category category)
    {
        var items = _toDoItemRepository.ViewAllValues().Where(x => x.Category == category).ToList();
        if (items.Count == 0) throw new CollectionCountIsZero();
        return ConvertToDto(items);
    }
    private List<ToDoItemDto> ConvertToDto(List<ToDoItem> list)
    {
        var l2 = new List<ToDoItemDto>();
        foreach (var item in list)
        {
            l2.Add(new ToDoItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                DueDate = item.DueDate,
                Category = item.Category,
                Priority = item.Priority,
                Status = item.Status
            });
        }

        return l2;
    }
}