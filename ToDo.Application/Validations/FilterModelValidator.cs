using FluentValidation;
using ToDo.Application.Models.ToDoItem;

namespace ToDo.Application.Validations;

public class FilterModelValidator : AbstractValidator<FilterModel>
{
    public FilterModelValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue)
            .WithMessage("Invalid status value.");
    }
}
