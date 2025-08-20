using FluentValidation;
using ToDo.Application.Models.ToDoItem;

namespace ToDo.Application.Validations;

public class SortModelValidator : AbstractValidator<SortModel>
{
    public SortModelValidator()
    {

        RuleFor(x => x.Priority)
            .IsInEnum()
            .When(x => x.Priority.HasValue)
            .WithMessage("Invalid priority value.");
    }
}
