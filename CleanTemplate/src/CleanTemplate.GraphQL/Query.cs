using System.Linq;
using System.Threading.Tasks;
using CleanTemplate.Application.Todos.Queries.GetTodoListIndex;
using CleanTemplate.Application.Todos.Queries.SearchTodos;
using HotChocolate;
using MediatR;

namespace CleanTemplate.GraphQL
{
    public class Query
    {
        public Task<IQueryable<SimplifiedTodoListVm>> GetTodoLists(
            [Service] IMediator mediator)
        {
            var query = new SearchTodosQuery();
            var result = mediator.Send(new SearchTodosQuery());
            return result;
        }
    }
}
