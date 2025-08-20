using ToDo.Application.Dtos;
using ToDo.Application.Models.ToDoItem;
using ToDo.Domain.Enums;

namespace ToDo.Application.Contracts.Services;

public interface IToDoItemService
{
    List<ToDoItemDto> ViewTasksByCategory(Category category);
    Task ChangeTaskStatus(ChangeTaskStatusModel model);
    Task<List<ToDoItemDto>> Sort(SortModel sortModel);
    Task<List<ToDoItemDto>> Filter(FilterModel sortModel);
    List<ToDoItemDto> ViewOverdueTasks();
    Task CreateItem(CreateModel model);
    void DeleteItem(long id);
    List<ToDoItemDto> ViewAllTasks();
}
