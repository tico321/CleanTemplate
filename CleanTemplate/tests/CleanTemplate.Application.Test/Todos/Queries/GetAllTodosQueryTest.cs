using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Queries.GetAll;
using Xunit;

namespace CleanTemplate.Application.Test.Todos.Queries
{
    public class GetAllTodosQueryTest : IClassFixture<MappingTestFixture>
    {
        public GetAllTodosQueryTest(MappingTestFixture mapping)
        {
            _mapping = mapping;
        }

        private readonly MappingTestFixture _mapping;

        [Fact]
        public async Task Handle_WithNoTodos_ReturnsEmpty()
        {
            var contextFactory = new ApplicationDbContextFactory();
            var context = await contextFactory.Create();
            var handler = new GetAllTodosQuery.GetAllTodosQueryHandler(context, _mapping.Mapper);

            var actual = await handler.Handle(new GetAllTodosQuery(), CancellationToken.None);

            Assert.NotNull(actual);
            Assert.Empty(actual.Todos);
        }

        [Fact]
        public async Task Handle_WithTodos_ReturnsAllTodos()
        {
            var contextFactory = new ApplicationDbContextFactory();
            var seederFunction = TodoSeeder.GetSeeder(TodoSeeder.DefaultTodoItems);
            var context = await contextFactory.Create(seederFunction);
            var handler = new GetAllTodosQuery.GetAllTodosQueryHandler(context, _mapping.Mapper);

            var actual = await handler.Handle(new GetAllTodosQuery(), CancellationToken.None);

            Assert.NotNull(actual);
            Assert.Equal(TodoSeeder.DefaultTodoItems.Count, actual.Todos.Count());
            Assert.Contains(actual.Todos, t => TodoSeeder.DefaultTodoItems[index: 0].Id == t.Id);
            Assert.Contains(actual.Todos, t => TodoSeeder.DefaultTodoItems[index: 0].Description == t.Description);
            Assert.Contains(actual.Todos, t => TodoSeeder.DefaultTodoItems[index: 1].Id == t.Id);
            Assert.Contains(actual.Todos, t => TodoSeeder.DefaultTodoItems[index: 1].Description == t.Description);
        }
    }
}
