using FluentValidation;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.Dtos;
using ToDo.Application.Exceptions.ToDoItem;
using ToDo.Application.Models.ToDoItem;
using ToDo.Domain.Entities;
using ToDo.Domain.Enums;

namespace ToDo.Application.Contracts.Services.Implementations;

public class ToDoItemService(IBaseRepository<ToDoItem> toItemRepository, IValidator<CreateModel> createValidation) : IToDoItemService
{
    private readonly IBaseRepository<ToDoItem> _toDoItemRepository = toItemRepository;
    private readonly IValidator<CreateModel> _createValidaton = createValidation;
    public void ChangeTaskStatus(long id, Status status)
    {
        try
        {
            var item = _toDoItemRepository.ViewAllValues().Where(x => x.Id == id).FirstOrDefault();
            if (item is null) throw new ExistException($"Item with id: {id} is not exist");
            item.Status = status;
            _toDoItemRepository.Save();
        }
        catch (ExistException e)
        {
            throw new Exception(e.Message);
        }
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
        catch (ValidationFailed e)
        {
            Console.WriteLine(e.Message);
        }
        catch (ExistException e)
        {
            throw new ExistException(e.Message);
        }
    }
    private void CheckToDoItemExist(string name)
    {
        var exist = _toDoItemRepository.ViewAllValues().Any(x => x.Name == name);
        if (exist) throw new ExistException($"To do item with {name} is already exist");
    }
    public void DeleteItem(long id)
    {
        try
        {
            var item = _toDoItemRepository.ViewAllValues().Where(x => x.Id == id).FirstOrDefault();
            if (item is null) throw new ExistException($"Item with id: {id} is not exist");
            _toDoItemRepository.DeleteTask(item);
            _toDoItemRepository.Save();
        }
        catch (ExistException e)
        {
            throw new ExistException(e.Message);
        }
    }
    public List<ToDoItemDto> Filter(FilterModel sortModel)
    {
        var items = _toDoItemRepository.ViewAllValues().Where(x => x.Status == sortModel.Status).ToList();
        return ConvertToDto(items);
    }
    public List<ToDoItemDto> Sort(SortModel sortModel)
    {
        if (sortModel.DueDate is not null)
            return GetByTasksByDueDate(sortModel.DueDate);
        if (sortModel.Priority is not null)
            return GetTasksByPriority(sortModel.Priority);

        return new List<ToDoItemDto>();
    }
    private List<ToDoItemDto> GetTasksByPriority(Priority? priority)
    {
        var items = _toDoItemRepository.ViewAllValues().Where(x => x.Priority == priority).ToList();
        if (items.Count == 0)
            return new List<ToDoItemDto>();
        return ConvertToDto(items);
    }
    private List<ToDoItemDto> GetByTasksByDueDate(DateTime? dueDate)
    {
        var items = _toDoItemRepository.ViewAllValues().Where(x => x.DueDate == dueDate || x.DueDate <= DateTime.Now).ToList();
        if (items.Count == 0)
            return new List<ToDoItemDto>();
        return ConvertToDto(items);
    }
    public List<ToDoItemDto> ViewAllTasks()
    {
        var items = _toDoItemRepository.ViewAllValues().ToList();
        return ConvertToDto(items);
    }
    public List<ToDoItemDto> ViewOverdueTasks()
    {
        var items = _toDoItemRepository.ViewAllValues().Where(x => x.DueDate < DateTime.Now).ToList();
        return ConvertToDto(items);
    }
    public List<ToDoItemDto> ViewTasksByCategory(Category category)
    {
        var items = _toDoItemRepository.ViewAllValues().Where(x => x.Category == category).ToList();
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