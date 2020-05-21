using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Domain.Todos;
using MediatR;

namespace CleanTemplate.Application.Todos.Commands.Create
{
    public class CreateTodoCommand : IRequest<long>
    {
        public string Description { get; set; }

        public class CreateTodoHandler : IRequestHandler<CreateTodoCommand, long>
        {
            private readonly IApplicationDbContext context;

            public CreateTodoHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<long> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
            {
                var todo = new TodoItem
                {
                    Description = request.Description
                };
                this.context.TodoItems.Add(todo);
                await this.context.SaveChangesAsync(cancellationToken);

                return todo.Id;
            }
        }
    }
}