using FluentValidation;

namespace CleanTemplate.Application.Todos.Commands.Create
{
    public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
    {
        public CreateTodoCommandValidator()
        {
            RuleFor(tc => tc.Description)
                .NotNull().WithMessage("Description cannot be null")
                .MaximumLength(maximumLength: 200).WithMessage("200 is the maximum length.")
                .MinimumLength(minimumLength: 5).WithMessage("5 is the minimum length");
        }
    }
}
