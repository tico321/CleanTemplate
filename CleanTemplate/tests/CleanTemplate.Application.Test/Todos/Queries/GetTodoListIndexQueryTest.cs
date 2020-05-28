using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Queries.GetTodoListIndex;
using Xunit;

namespace CleanTemplate.Application.Test.Todos.Queries
{
    public class GetTodoListIndexQueryTest : IClassFixture<MappingTestFixture>
    {
        public GetTodoListIndexQueryTest(MappingTestFixture mapping)
        {
            _mapping = mapping;
        }

        private readonly MappingTestFixture _mapping;

        [Fact]
        public async Task Handle_WithNoTodos_ReturnsEmpty()
        {
            var contextFactory = new ApplicationDbContextFactory();
            var context = await contextFactory.Create();
            var handler = new GetTodoListIndexQuery.Handler(context, _mapping.Mapper);

            var actual = await handler.Handle(new GetTodoListIndexQuery(), CancellationToken.None);

            Assert.NotNull(actual);
            Assert.Empty(actual.Todos);
        }

        [Fact]
        public async Task Handle_WithTodos_ReturnsAllTodos()
        {
            var contextFactory = new ApplicationDbContextFactory();
            var seederFunction = TodoSeeder.GetSeeder(TodoSeeder.DefaultTodoLists);
            var context = await contextFactory.Create(seederFunction);
            var handler = new GetTodoListIndexQuery.Handler(context, _mapping.Mapper);

            var actual = await handler.Handle(new GetTodoListIndexQuery(), CancellationToken.None);

            Assert.NotNull(actual);
            Assert.Equal(TodoSeeder.DefaultTodoLists.Count, actual.Todos.Count());
            var todoList1 =
                actual.Todos.FirstOrDefault(t => t.Description == TodoSeeder.DefaultTodoLists[index: 0].Description);
            var todoList2 =
                actual.Todos.FirstOrDefault(t => t.Description == TodoSeeder.DefaultTodoLists[index: 1].Description);
            Assert.NotNull(todoList1);
            Assert.Equal(TodoSeeder.DefaultTodoLists[index: 0].DisplayOrder, todoList1.DisplayOrder);
            Assert.Equal(TodoSeeder.DefaultTodoLists[index: 0].Todos.Count(), todoList1.Count);
            Assert.NotNull(todoList2);
            Assert.Equal(TodoSeeder.DefaultTodoLists[index: 1].DisplayOrder, todoList2.DisplayOrder);
            Assert.Equal(TodoSeeder.DefaultTodoLists[index: 1].Todos.Count(), todoList2.Count);
            var todos = actual.Todos.ToList();
            for (var i = 1; i < todos.Count; i++)
            {
                var current = todos[i];
                var previous = todos[i - 1];
                Assert.True(current.DisplayOrder > previous.DisplayOrder);
            }
        }
    }
}
