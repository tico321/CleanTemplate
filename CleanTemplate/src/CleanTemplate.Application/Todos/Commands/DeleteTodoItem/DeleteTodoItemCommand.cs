using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.CrossCuttingConcerns.JSON;
using CleanTemplate.Application.Todos.Queries.QueryObjects;
using MediatR;

namespace CleanTemplate.Application.Todos.Commands.DeleteTodoItem
{
    public class DeleteTodoItemCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int ItemId { get; set; }

        public class Handler : IRequestHandler<DeleteTodoItemCommand, bool>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
            {
                var todoList = await _context.TodoLists.GetTodoListById(request.Id, cancellationToken);
                if (todoList == null)
                {
                    throw new NotFoundException(request.ToJson());
                }

                if (!todoList.RemoveTodo(request.ItemId))
                {
                    throw new NotFoundException(request.ToJson());
                }

                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
    }
}
