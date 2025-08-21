using FluentValidation;
using FluentValidation.Results;
using Moq;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.Contracts.Services.Implementations;
using ToDo.Application.Exceptions.ToDoItem;
using ToDo.Application.Models.ToDoItem;
using ToDo.Domain.Entities;
using ToDo.Domain.Enums;

namespace ToDo.XUnitTest.ServiceTests;

public class ToDoServiceTest
{
    private readonly Mock<IBaseRepository<ToDoItem>> _toDoItemRepositoryMock;
    private readonly Mock<IValidator<CreateModel>> _createValidator;
    private readonly Mock<IValidator<ChangeTaskStatusModel>> _changeTaskStatusValidator;
    private readonly Mock<IValidator<FilterModel>> _filterValidator;
    private readonly Mock<IValidator<SortModel>> _sortValidator;
    private readonly ToDoItemService _toDoItemService;
    public ToDoServiceTest()
    {
        _toDoItemRepositoryMock = new Mock<IBaseRepository<ToDoItem>>();
        _createValidator = new Mock<IValidator<CreateModel>>();
        _changeTaskStatusValidator = new Mock<IValidator<ChangeTaskStatusModel>>();
        _filterValidator = new Mock<IValidator<FilterModel>>();
        _sortValidator = new Mock<IValidator<SortModel>>();


        _toDoItemService = new ToDoItemService(_toDoItemRepositoryMock.Object,
            _createValidator.Object,
            _changeTaskStatusValidator.Object,
            _filterValidator.Object,
            _sortValidator.Object);
    }




    [Fact]
    public void ViewTasksByCategory_ShouldThrowException_WhenNoTasksExist()
    {
        var tasks = new List<ToDoItem>
        {

        };

        _toDoItemRepositoryMock.Setup(r => r.ViewAllValues()).Returns(tasks.AsQueryable());

        Assert.Throws<CollectionCountIsZero>(() => _toDoItemService.ViewTasksByCategory(Category.Work));
    }


    [Fact]
    public void ViewTasksByCategory_ShouldReturnTasks_WhenTasksExist()
    {
        var tasks = new List<ToDoItem>
        {
            new ToDoItem { Id = 1, Name = "Task1", Category = Category.Work, Priority = Priority.Low, Status = Status.InProgress },
            new ToDoItem { Id = 1, Name = "Task1", Category = Category.Work, Priority = Priority.Low, Status = Status.InProgress },
            new ToDoItem { Id = 1, Name = "Task1", Category = Category.Personal, Priority = Priority.Low, Status = Status.InProgress },

        };

        _toDoItemRepositoryMock.Setup(r => r.ViewAllValues()).Returns(tasks.AsQueryable());

        var result = _toDoItemService.ViewTasksByCategory(Category.Work);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.Equal(Category.Work, t.Category));
    }



    [Fact]
    public void ViewOverdueTasks_ShouldThrowException_WhenNoOverdueTasksExist()
    {
        var now = DateTime.Now;
        var tasks = new List<ToDoItem>
        {
            new ToDoItem { Id = 3, Name = "Future Task", DueDate = now.AddDays(2), Category = Category.Personal, Priority =  Priority.Medium, Status = Status.InProgress },
            new ToDoItem { Id = 3, Name = "Future Task", DueDate = now.AddDays(2), Category = Category.Personal, Priority =  Priority.Medium, Status = Status.InProgress }
        };

        _toDoItemRepositoryMock.Setup(r => r.ViewAllValues()).Returns(tasks.AsQueryable());

        Assert.Throws<CollectionCountIsZero>(() => _toDoItemService.ViewOverdueTasks());
    }


    [Fact]
    public void ViewOverdueTasks_ShouldReturnTasks_WhenOverdueTasksExist()
    {
        var now = DateTime.Now;
        var tasks = new List<ToDoItem>
        {
            new ToDoItem { Id = 1, Name = "Old Task1", DueDate = now.AddDays(-2), Category = Category.Study, Priority = Priority.Low, Status =  Status.Completed },
            new ToDoItem { Id = 2, Name = "Old Task2", DueDate = now.AddDays(-1), Category = Category.Work, Priority = Priority.Low, Status = Status.InProgress },
            new ToDoItem { Id = 3, Name = "Future Task", DueDate = now.AddDays(2), Category = Category.Personal, Priority =  Priority.Medium, Status = Status.InProgress }
        };

        _toDoItemRepositoryMock.Setup(r => r.ViewAllValues()).Returns(tasks.AsQueryable());

        var result = _toDoItemService.ViewOverdueTasks();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.True(t.DueDate < now));
    }

    [Fact]
    public void ViewAllTasks_ShouldReturnTasks_WhenTasksExist()
    {
        var tasks = new List<ToDoItem>
        {
            new ToDoItem { Id = 1, Name = "Task1", Priority = Priority.High, DueDate = DateTime.Now, Category = Category.Work, Status = Status.Completed },
            new ToDoItem { Id = 2, Name = "Task2", Priority = Priority.Low, DueDate = DateTime.Now.AddDays(1), Category = Category.Work, Status = Status.InProgress }
        };

        _toDoItemRepositoryMock.Setup(r => r.ViewAllValues()).Returns(tasks.AsQueryable());

        var result = _toDoItemService.ViewAllTasks();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Task1", result[0].Name);
        Assert.Equal("Task2", result[1].Name);
    }





    [Fact]
    public void GetByTasksByDueDate_ShouldThrowException_WhenNoTasksExist()
    {
        var tasks = new List<ToDoItem>
        {
            new ToDoItem { Id = 1, Name = "Task1", DueDate = new DateTime(2025, 08, 25), Category=Category.Work, Priority= Priority.Low, Status = Status.Completed }
        };

        _toDoItemRepositoryMock.Setup(r => r.ViewAllValues()).Returns(tasks.AsQueryable());

        var dueDate = new DateTime(2025, 08, 20);

        Assert.Throws<CollectionCountIsZero>(() => _toDoItemService.GetByTasksByDueDate(dueDate));
    }


    [Fact]
    public void GetByTasksByDueDate_ShouldReturnTasks_WhenTasksExist()
    {
        var tasks = new List<ToDoItem>
        {
            new ToDoItem { Id = 1, Name = "Test1", Priority = Priority.High, Category=Category.Personal, Status=Status.Completed },
            new ToDoItem { Id = 1, Name = "Test1", Priority = Priority.High, Category=Category.Work, Status=Status.InProgress },
        };

        _toDoItemRepositoryMock.Setup(r => r.ViewAllValues()).Returns(tasks.AsQueryable());

        var dueDate = new DateTime(2025, 08, 21);

        var result = _toDoItemService.GetByTasksByDueDate(dueDate);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.True(t.DueDate <= dueDate));
    }



    [Fact]
    public void GetTasksByPriority_ShouldThrowException_WhenNoTasksExist()
    {
        var tasks = new List<ToDoItem>();
        _toDoItemRepositoryMock.Setup(r => r.ViewAllValues()).Returns(tasks.AsQueryable());

        Assert.Throws<CollectionCountIsZero>(() => _toDoItemService.GetTasksByPriority(Priority.Low));
    }



    [Fact]
    public void GetTasksByPriority_ShouldReturnTasks_WhenTasksExist()
    {

        var tasks = new List<ToDoItem>
        {
            new ToDoItem { Id = 1, Name = "Test1", Priority = Priority.High, Category=Category.Personal, Status=Status.Completed },
            new ToDoItem { Id = 1, Name = "Test1", Priority = Priority.High, Category=Category.Work, Status=Status.InProgress },
        };

        _toDoItemRepositoryMock.Setup(r => r.ViewAllValues()).Returns(tasks.AsQueryable());

        var result = _toDoItemService.GetTasksByPriority(Priority.High);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.Equal(Priority.High, t.Priority));
    }



    [Fact]
    public async Task Sort_ShouldThrowValidationFailed_WhenModelIsInvalid()
    {
        var result = new ValidationResult(new List<ValidationFailure> {
            new ValidationFailure("DueDate", "DueDate required")
        });

        _sortValidator
            .Setup(v => v.ValidateAsync(It.IsAny<SortModel>(), default))
            .ReturnsAsync(result);

        var model = new SortModel();

        await Assert.ThrowsAsync<ValidationFailed>(() => _toDoItemService.Sort(model));
    }



    [Fact]
    public async Task Filter_ShouldReturnDtos_WhenItemsFound()
    {
        var model = new FilterModel { Status = Status.Completed };

        _filterValidator
            .Setup(v => v.ValidateAsync(model, default))
            .ReturnsAsync(new ValidationResult());

        var items = new List<ToDoItem>
        {
            new ToDoItem { Id = 1, Name = "Task 1", Status = Status.Completed, Category=Category.Work, Priority=Priority.Low},
            new ToDoItem { Id = 2, Name = "Task 2", Status = Status.Completed, Category=Category.Study, Priority= Priority.Medium }
        };

        _toDoItemRepositoryMock
            .Setup(r => r.ViewAllValues())
            .Returns(items.AsQueryable());

        var result = await _toDoItemService.Filter(model);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, dto => Assert.Equal(Status.Completed, dto.Status));
    }


    [Fact]
    public async Task Filter_ShouldThrowCollectionCountIsZero_WhenNoItemsFound()
    {

        var model = new FilterModel { Status = Status.Completed };

        _filterValidator
            .Setup(v => v.ValidateAsync(model, default))
            .ReturnsAsync(new ValidationResult());

        _toDoItemRepositoryMock
            .Setup(r => r.ViewAllValues())
            .Returns(new List<ToDoItem>().AsQueryable());

        await Assert.ThrowsAsync<CollectionCountIsZero>(() => _toDoItemService.Filter(model));
    }


    [Fact]
    public async Task Filter_ShouldThrowValidationFailed_WhenInvalidModel()
    {
        var model = new FilterModel { Status = Status.Completed };

        var errors = new List<ValidationFailure>
        {
            new ValidationFailure("Status", "Invalid status")
        };

        _filterValidator
            .Setup(v => v.ValidateAsync(model, default))
            .ReturnsAsync(new ValidationResult(errors));

        await Assert.ThrowsAsync<ValidationFailed>(() => _toDoItemService.Filter(model));
    }




    [Fact]
    public void DeleteItem_ShouldDeleteAndSave_WhenItemExists()
    {
        long id = 1;
        var item = new ToDoItem { Id = id, Name = "Task 1", Category = Category.Study, Priority = Priority.Low, Status = Status.Completed };

        _toDoItemRepositoryMock
            .Setup(r => r.ViewAllValues())
            .Returns(new List<ToDoItem> { item }.AsQueryable());

        _toDoItemService.DeleteItem(id);

        _toDoItemRepositoryMock.Verify(r => r.DeleteTask(item), Times.Once);
        _toDoItemRepositoryMock.Verify(r => r.Save(), Times.Once);
    }



    [Fact]
    public void DeleteItem_ShouldThrowException_WhenItemNotFound()
    {
        long id = 100;

        _toDoItemRepositoryMock
            .Setup(r => r.ViewAllValues())
            .Returns(new List<ToDoItem>().AsQueryable());

        Assert.Throws<ExistException>(() => _toDoItemService.DeleteItem(id));
    }


    [Fact]
    public void DeleteItem_ShouldThrowException_WhenIdIsNegative()
    {
        long invalidId = -1;

        Assert.Throws<IdIsNotCorrectFromatException>(() => _toDoItemService.DeleteItem(invalidId));
    }





    [Fact]
    public async Task CreateItem_ShouldThrowValidationFailed_WhenInvalid()
    {
        var model = new CreateModel { Name = "" };

        var errors = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required")
        };

        _createValidator
            .Setup(v => v.ValidateAsync(model, default))
            .ReturnsAsync(new ValidationResult(errors));

        await Assert.ThrowsAsync<ValidationFailed>(() => _toDoItemService.CreateItem(model));
    }

    [Fact]
    public async Task CreateItem_ShouldAddNewItem_WhenValid()
    {
        var model = new CreateModel
        {
            Name = "New Task",
            Category = Category.Work,
            Status = Status.Completed,
            Description = "Test desc",
            Priority = Priority.Medium,
            DueDate = DateTime.Now.AddDays(1)
        };

        _createValidator
            .Setup(v => v.ValidateAsync(model, default))
            .ReturnsAsync(new ValidationResult());

        await _toDoItemService.CreateItem(model);

        _toDoItemRepositoryMock.Verify(r => r.AddTask(It.Is<ToDoItem>(t => t.Name == "New Task")), Times.Once);
        _toDoItemRepositoryMock.Verify(r => r.Save(), Times.Once);
    }



    [Fact]
    public async Task ChangeTaskStatus_ShouldUpdateStatus_WhenValid()
    {
        var model = new ChangeTaskStatusModel { Id = 1, Status = Status.Completed };

        _changeTaskStatusValidator
            .Setup(v => v.ValidateAsync(model, default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var item = new ToDoItem { Id = 1, Status = Status.NotStarted, Category = Category.Work, Name = "task", Priority = Priority.Low };

        _toDoItemRepositoryMock
            .Setup(r => r.ViewAllValues())
            .Returns(new List<ToDoItem> { item }.AsQueryable());

        await _toDoItemService.ChangeTaskStatus(model);

        Assert.Equal(Status.Completed, item.Status);
        _toDoItemRepositoryMock.Verify(r => r.Save(), Times.Once);
    }


    [Fact]
    public async Task ChangeTaskStatus_ShouldThrowValidationFailed_WhenInvalid()
    {
        var model = new ChangeTaskStatusModel { Id = 1, Status = Status.Completed };

        var errors = new List<ValidationFailure> { new ValidationFailure("Status", "Invalid status") };
        _changeTaskStatusValidator
            .Setup(v => v.ValidateAsync(model, default))
            .ReturnsAsync(new ValidationResult(errors));

        await Assert.ThrowsAsync<ValidationFailed>(() => _toDoItemService
        .ChangeTaskStatus(model));
    }



    [Fact]
    public async Task ChangeTaskStatus_ShouldThrowExistException_WhenItemNotFound()
    {
        var model = new ChangeTaskStatusModel { Id = 1, Status = Status.Completed };

        _changeTaskStatusValidator
            .Setup(v => v.ValidateAsync(model, default))
            .ReturnsAsync(new ValidationResult());

        _toDoItemRepositoryMock
            .Setup(r => r.ViewAllValues())
            .Returns(new List<ToDoItem>().AsQueryable());

        await Assert.ThrowsAsync<ExistException>(() => _toDoItemService.ChangeTaskStatus(model));
    }

}
