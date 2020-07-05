using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.CrossCuttingConcerns.JSON;
using CleanTemplate.Application.CrossCuttingConcerns.Persistence;
using CleanTemplate.Application.Todos.Model;
using CleanTemplate.Application.Todos.Queries.QueryObjects;
using CleanTemplate.SharedKernel.Common;
using FluentValidation;
using MediatR;

namespace CleanTemplate.Application.Todos.Commands.UpdateTodoItem
{
    public class UpdateTodoItemCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public string State { get; set; }

        public class Validator : AbstractValidator<UpdateTodoItemCommand>
        {
            public Validator()
            {
                RuleFor(c => c.Description)
                    .NotNull()
                    .WithMessage("Description cannot be null")
                    .MaximumLength(200)
                    .WithMessage("200 is the maximum length.")
                    .MinimumLength(5)
                    .WithMessage("5 is the minimum length");
                RuleFor(c => c.State)
                    .Must(Enumeration.IsValidName<TodoItemState>)
                    .WithMessage("Not a valid state");
            }
        }

        public class Handler : IRequestHandler<UpdateTodoItemCommand, bool>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
            {
                var item = await _context.TodoLists.GetTodoItemById(request.Id, request.ItemId, cancellationToken);
                if (item == null)
                {
                    throw new NotFoundException(request.ToJson());
                }

                _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
