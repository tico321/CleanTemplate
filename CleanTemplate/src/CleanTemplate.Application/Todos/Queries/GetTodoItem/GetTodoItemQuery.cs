using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.CrossCuttingConcerns.JSON;
using CleanTemplate.Application.Todos.Queries.GetTodoList;
using CleanTemplate.Application.Todos.Queries.QueryObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Todos.Queries.GetTodoItem
{
    public class GetTodoItemQuery : IRequest<TodoItemVm>
    {
        public int Id { get; set; }
        public int ItemId { get; set; }

        public class Handler : IRequestHandler<GetTodoItemQuery, TodoItemVm>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TodoItemVm> Handle(GetTodoItemQuery request, CancellationToken cancellationToken)
            {
                var result = await _context.TodoLists
                    .AsNoTracking()
                    .GetTodoItemById(request.Id, request.ItemId, cancellationToken);
                if (result == null)
                {
                    throw new NotFoundException(request.ToJson());
                }

                return _mapper.Map<TodoItemVm>(result);
            }
        }
    }
}
