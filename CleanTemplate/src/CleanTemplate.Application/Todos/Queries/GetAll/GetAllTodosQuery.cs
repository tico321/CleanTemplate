using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Todos.Queries.GetAll
{
    public class GetAllTodosQuery : IRequest<TodoListVm>
    {
        public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, TodoListVm>
        {
            private readonly IApplicationDbContext context;

            public GetAllTodosQueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<TodoListVm> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
            {
                var todos = await this.context.TodoItems
                    .Select(t => new TodoVm { Id = t.Id, Description = t.Description})
                    .ToListAsync(cancellationToken);
                return new TodoListVm{ Todos = todos };
            }
        }
    }
}