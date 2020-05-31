using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.Todos.Queries.GetTodoListById;
using CleanTemplate.Application.Todos.Queries.GetTodoListIndex;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using MediatR;

namespace CleanTemplate.GraphQL
{
    public class Query
    {
        // Always define the attributes in this order Paging -> Filtering -> Sorting -> Field Resolver https://hotchocolate.io/docs/filters
        [UsePaging, UseFiltering]
        //[UseSorting(SortType = typeof(TodoListSortType))] //review why it's not working
        public Task<IQueryable<SimplifiedTodoListVm>> GetTodoLists([Service] IMediator mediator)
        {
            return mediator.Send(new GetTodoListIndexQuery());
        }

        public Task<TodoListVm> GetTodoListById([Service] IMediator mediator, int id, IResolverContext context)
        {
            return context.CacheDataLoader<int, TodoListVm>(
                    "todoListById",
                    (key, token) => mediator.Send(new GetTodoListQuery { Id = id }, token))
                .LoadAsync(id, CancellationToken.None);
        }
    }
}
