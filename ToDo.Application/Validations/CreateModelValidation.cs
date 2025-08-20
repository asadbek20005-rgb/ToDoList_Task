using FluentValidation;
using ToDo.Application.Models.ToDoItem;

namespace ToDo.Application.Validations;

public class CreateModelValidation : AbstractValidator<CreateModel>
{
    public CreateModelValidation()
    {
        RuleFor(x => x.Name)
             .NotEmpty().WithMessage("Name is required")
             .MinimumLength(3).WithMessage("Name must be at least 3 characters long")
             .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot be longer than 500 characters");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Priority value is invalid");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.Now).WithMessage("Due date must be in the future");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Category value is invalid");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status value is invalid");

    }
}
