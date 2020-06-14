using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns.Persistence;
using CleanTemplate.Application.Todos.Queries.QueryObjects;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Todos.Commands.AddTodoItem
{
    public class AddTodoItemCommand : IRequest<int>
    {
        public int TodoListId { get; set; }
        public string Description { get; set; }

        public class Validator : AbstractValidator<AddTodoItemCommand>
        {
            public Validator(IApplicationDbContext context)
            {
                RuleFor(c => c.Description)
                    .NotNull().WithMessage("Description cannot be null")
                    .MaximumLength(maximumLength: 200).WithMessage("200 is the maximum length.")
                    .MinimumLength(minimumLength: 5).WithMessage("5 is the minimum length");

                RuleFor(c => c.TodoListId)
                    .MustAsync((id, cancellationToken) => context.TodoLists.AnyAsync(t => t.Id == id))
                    .WithMessage("Invalid TodoListId");
            }
        }

        public class Handler : IRequestHandler<AddTodoItemCommand, int>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
            {
                var todoList = await _context.TodoLists.GetTodoListById(request.TodoListId, cancellationToken);
                var item = todoList.AddTodo(request.Description);
                await _context.SaveChangesAsync(cancellationToken);
                return item.Id;
            }
        }
    }
}
