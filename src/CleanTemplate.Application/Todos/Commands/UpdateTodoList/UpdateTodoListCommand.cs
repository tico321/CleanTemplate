using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.CrossCuttingConcerns.Persistence;
using CleanTemplate.Application.Todos.Queries.QueryObjects;
using FluentValidation;
using MediatR;

namespace CleanTemplate.Application.Todos.Commands.UpdateTodoList
{
    public class UpdateTodoListCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }

        public class Validator : AbstractValidator<UpdateTodoListCommand>
        {
            public Validator()
            {
                RuleFor(tc => tc.Description)
                    .NotNull().WithMessage("Description cannot be null")
                    .MaximumLength(maximumLength: 200).WithMessage("200 is the maximum length.")
                    .MinimumLength(minimumLength: 5).WithMessage("5 is the minimum length");
            }
        }

        public class Handler : IRequestHandler<UpdateTodoListCommand, bool>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
            {
                var todoList = await _context.TodoLists.GetTodoListById(request.Id, cancellationToken);
                if (todoList == null)
                {
                    throw new NotFoundException(request.Id.ToString());
                }

                _mapper.Map(request, todoList);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
    }
}
