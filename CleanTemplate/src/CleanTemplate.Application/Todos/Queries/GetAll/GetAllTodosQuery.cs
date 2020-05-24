using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanTemplate.Application.CrossCuttingConcerns;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Todos.Queries.GetAll
{
    public class GetAllTodosQuery : IRequest<TodoListVm>
    {
        public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, TodoListVm>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetAllTodosQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                this._context = context;
                _mapper = mapper;
            }

            public async Task<TodoListVm> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
            {
                var todos = await _context.TodoItems
                    .ProjectTo<TodoVm>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return new TodoListVm {Todos = todos};
            }
        }
    }
}
