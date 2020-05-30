using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Application.Todos.Queries.GetTodoListIndex;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Todos.Queries.SearchTodos
{
    public class SearchTodosQuery : IRequest<IQueryable<SimplifiedTodoListVm>>
    {
        public class GetQueryableTodosQueryHandler : IRequestHandler<SearchTodosQuery, IQueryable<SimplifiedTodoListVm>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetQueryableTodosQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public Task<IQueryable<SimplifiedTodoListVm>> Handle(
                SearchTodosQuery request,
                CancellationToken cancellationToken)
            {
                var query = _context.TodoLists
                    .AsNoTracking()
                    .OrderBy(t => t.DisplayOrder)
                    .ProjectTo<SimplifiedTodoListVm>(_mapper.ConfigurationProvider);

                return Task.FromResult(query);
            }
        }
    }
}
