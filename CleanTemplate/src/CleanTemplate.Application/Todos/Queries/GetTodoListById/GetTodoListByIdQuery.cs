using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.CrossCuttingConcerns.Persistence;
using CleanTemplate.Application.Todos.Queries.QueryObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Todos.Queries.GetTodoListById
{
    public class GetTodoListQuery : IRequest<TodoListVm>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<GetTodoListQuery, TodoListVm>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TodoListVm> Handle(GetTodoListQuery request, CancellationToken cancellationToken)
            {
                var todoList = await _context.TodoLists.AsNoTracking().GetTodoListById(request.Id, cancellationToken);
                if (todoList == null)
                {
                    throw new NotFoundException(request.Id.ToString());
                }

                todoList.SortTodosByDisplayOrder();
                var vm = _mapper.Map<TodoListVm>(todoList);
                return vm;
            }
        }
    }
}
