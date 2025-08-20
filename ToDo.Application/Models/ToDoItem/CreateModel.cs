using System.ComponentModel.DataAnnotations;
using ToDo.Domain.Enums;

namespace ToDo.Application.Models.ToDoItem;

public class CreateModel
{
    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Priority Priority { get; set; }
    public DateTime DueDate { get; set; }
    public Category Category { get; set; }
    public Status Status { get; set; }
}
