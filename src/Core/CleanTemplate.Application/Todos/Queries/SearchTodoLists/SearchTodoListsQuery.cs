using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanTemplate.Application.CrossCuttingConcerns.Persistence;
using CleanTemplate.Application.Todos.Queries.GetTodoListIndex;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Todos.Queries.SearchTodoLists
{
    public class SearchTodoListsQuery : IRequest<Page<SimplifiedTodoListVm>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public class Validator : AbstractValidator<SearchTodoListsQuery>
        {
            public Validator()
            {
                RuleFor(s => s.Page)
                    .GreaterThan(0)
                    .WithMessage("Page cannot be negative");
                RuleFor(s => s.PageSize)
                    .GreaterThan(4)
                    .WithMessage("PageSize min value is 5");
            }
        }

        public class Handler : IRequestHandler<SearchTodoListsQuery, Page<SimplifiedTodoListVm>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public Task<Page<SimplifiedTodoListVm>> Handle(
                SearchTodoListsQuery request,
                CancellationToken cancellationToken)
            {
                var query = _context.TodoLists
                    .AsNoTracking()
                    .OrderBy(t => t.DisplayOrder)
                    .ProjectTo<SimplifiedTodoListVm>(_mapper.ConfigurationProvider)
                    .WithPaging(new PageQuery(request.Page, request.PageSize), cancellationToken);
                return query;
            }
        }
    }
}
