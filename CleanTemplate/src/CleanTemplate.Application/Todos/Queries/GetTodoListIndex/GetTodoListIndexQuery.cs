using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanTemplate.Application.CrossCuttingConcerns;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Todos.Queries.GetTodoListIndex
{
    public class GetTodoListIndexQuery : IRequest<TodoListIndexResponse>
    {
        public class Handler : IRequestHandler<GetTodoListIndexQuery, TodoListIndexResponse>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TodoListIndexResponse> Handle(
                GetTodoListIndexQuery request,
                CancellationToken cancellationToken)
            {
                var todos = await _context.TodoLists
                    .AsNoTracking()
                    .OrderBy(t => t.DisplayOrder)
                    .ProjectTo<SimplifiedTodoListVm>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return new TodoListIndexResponse {Todos = todos};
            }
        }
    }
}
