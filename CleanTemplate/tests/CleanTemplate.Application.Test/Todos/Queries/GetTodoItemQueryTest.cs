using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Model;
using CleanTemplate.Application.Todos.Queries.GetTodoItem;
using Xunit;

namespace CleanTemplate.Application.Test.Todos.Queries
{
    public class GetTodoItemQueryTest : IClassFixture<RequestTestFixture>
    {
        public GetTodoItemQueryTest(RequestTestFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly RequestTestFixture _fixture;

        [Fact]
        public async Task GetTodoItem()
        {
            var itemDescription = "todo1";
            var todos = new List<TodoList>
            {
                new TodoList("userId", "desc", displayOrder: 0).SequenceAddTodo(itemDescription)
            };
            await TodoSeeder.GetSeeder(todos)(_fixture.Context);
            var todoList = _fixture.Context.TodoLists.First();
            var query = new GetTodoItemQuery {Id = todoList.Id, ItemId = todoList.Todos.First().Id};
            var handler = new GetTodoItemQuery.Handler(_fixture.Context, _fixture.Mapper);

            var item = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(item);
            Assert.Equal(itemDescription, item.Description);
            Assert.Equal(TodoItemState.Pending.Name, item.State);
            Assert.Equal(expected: 1, item.DisplayOrder);
        }

        [Fact]
        public async Task GetTodoItem_ItemNotFound()
        {
            var todos = new List<TodoList> {new TodoList("userId", "desc2", displayOrder: 0)};
            await TodoSeeder.GetSeeder(todos)(_fixture.Context);
            var todoList = _fixture.Context.TodoLists.First();
            var query = new GetTodoItemQuery {Id = todoList.Id, ItemId = 2};
            var handler = new GetTodoItemQuery.Handler(_fixture.Context, _fixture.Mapper);

            try
            {
                var item = await handler.Handle(query, CancellationToken.None);
                Assert.False(condition: true, "Should throw not found exception");
            }
            catch (NotFoundException e)
            {
                Assert.Contains("2", e.Message);
            }
        }

        [Fact]
        public async Task GetTodoItem_ListNotFound()
        {
            var query = new GetTodoItemQuery {Id = 10, ItemId = 2};
            var handler = new GetTodoItemQuery.Handler(_fixture.Context, _fixture.Mapper);

            try
            {
                var item = await handler.Handle(query, CancellationToken.None);
                Assert.False(condition: true, "Should throw not found exception");
            }
            catch (NotFoundException e)
            {
                Assert.Contains("10", e.Message);
            }
        }
    }
}
