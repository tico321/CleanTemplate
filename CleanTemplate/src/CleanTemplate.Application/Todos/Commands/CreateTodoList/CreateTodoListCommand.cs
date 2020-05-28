using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Application.Todos.Model;
using CleanTemplate.Application.Todos.Queries.QueryObjects;
using FluentValidation;
using MediatR;

namespace CleanTemplate.Application.Todos.Commands.CreateTodoList
{
    public class CreateTodoListCommand : IRequest<int>
    {
        public string Description { get; set; }

        public class Validator : AbstractValidator<CreateTodoListCommand>
        {
            public Validator()
            {
                RuleFor(tc => tc.Description)
                    .NotNull().WithMessage("Description cannot be null")
                    .MaximumLength(maximumLength: 200).WithMessage("200 is the maximum length.")
                    .MinimumLength(minimumLength: 5).WithMessage("5 is the minimum length");
            }
        }

        public class Handler : IRequestHandler<CreateTodoListCommand, int>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;

            public Handler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
            {
                var displayOrder =
                    await _context.TodoLists.CountTodoLists(_currentUserService.UserId, cancellationToken);
                var todo = new TodoList(_currentUserService.UserId, request.Description, displayOrder + 1);
                _context.TodoLists.Add(todo);
                await _context.SaveChangesAsync(cancellationToken);

                return todo.Id;
            }
        }
    }
}
