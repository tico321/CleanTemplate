using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.CrossCuttingConcerns.Persistence;
using MediatR;

namespace CleanTemplate.Application.Todos.Commands.DeleteTodoList
{
    public class DeleteTodoListCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<DeleteTodoListCommand, bool>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
            {
                var todoList = await _context.TodoLists.FindAsync(request.Id);
                if (todoList == null)
                {
                    throw new NotFoundException(request.Id.ToString());
                }

                _context.TodoLists.Remove(todoList);

                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
    }
}
