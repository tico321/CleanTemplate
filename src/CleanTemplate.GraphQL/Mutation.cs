using System.Threading.Tasks;
using CleanTemplate.Application.Todos.Commands.AddTodoItem;
using CleanTemplate.Application.Todos.Commands.CreateTodoList;
using CleanTemplate.Application.Todos.Commands.DeleteTodoItem;
using CleanTemplate.Application.Todos.Commands.DeleteTodoList;
using CleanTemplate.Application.Todos.Commands.UpdateTodoItem;
using CleanTemplate.Application.Todos.Commands.UpdateTodoList;
using HotChocolate;
using MediatR;

namespace CleanTemplate.GraphQL
{
    public class Mutation
    {
        public Task<int> CreteTodoList([Service] IMediator mediator, CreateTodoListCommand command)
        {
            return mediator.Send(command);
        }

        public Task<bool> DeleteTodoList([Service] IMediator mediator, int id)
        {
            return mediator.Send(new DeleteTodoListCommand {Id = id});
        }

        public Task<bool> UpdateTodoList([Service] IMediator mediator, UpdateTodoListCommand command)
        {
            return mediator.Send(command);
        }

        public Task<int> CreateTodoItem([Service] IMediator mediator, AddTodoItemCommand command)
        {
            return mediator.Send(command);
        }

        public Task<bool> DeleteTodoItem([Service] IMediator mediator, int id)
        {
            return mediator.Send(new DeleteTodoItemCommand {Id = id});
        }

        public Task<bool> UpdateTodoItem([Service] IMediator mediator, UpdateTodoItemCommand command)
        {
            return mediator.Send(command);
        }
    }
}
