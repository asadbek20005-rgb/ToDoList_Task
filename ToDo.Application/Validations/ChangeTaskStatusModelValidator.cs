using FluentValidation;
using ToDo.Application.Models.ToDoItem;

namespace ToDo.Application.Validations;

public class ChangeTaskStatusModelValidator : AbstractValidator<ChangeTaskStatusModel>
{
    public ChangeTaskStatusModelValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Task Id must be greater than 0.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid status value.");

    }
}
