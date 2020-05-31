using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanTemplate.Application.CrossCuttingConcerns.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Todos.Queries.GetTodoListIndex
{
    // We should try to avoid returning IQueryable<> results but we do it for convenience for GraphQL.
    public class GetTodoListIndexQuery : IRequest<IQueryable<SimplifiedTodoListVm>>
    {
        public class Handler : IRequestHandler<GetTodoListIndexQuery, IQueryable<SimplifiedTodoListVm>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public Task<IQueryable<SimplifiedTodoListVm>> Handle(
                GetTodoListIndexQuery request,
                CancellationToken cancellationToken)
            {
                var query = _context.TodoLists
                    .AsNoTracking()
                    .ProjectTo<SimplifiedTodoListVm>(_mapper.ConfigurationProvider);

                return Task.FromResult(query);
            }
        }
    }
}
