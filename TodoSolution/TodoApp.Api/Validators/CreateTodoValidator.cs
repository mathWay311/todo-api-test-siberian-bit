using FluentValidation;
using TodoApp.Api.Models;

namespace TodoApp.Api.Validators;

public class CreateTodoValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Заголовок не может быть пустым")
            .MaximumLength(200).WithMessage("Заголовок не должен превышать 200 символов");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Описание не должно превышать 1000 символов");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Некорректный приоритет");
    }
}