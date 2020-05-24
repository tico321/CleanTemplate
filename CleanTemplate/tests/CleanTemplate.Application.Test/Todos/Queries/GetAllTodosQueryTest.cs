using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Queries.GetAll;
using Xunit;

namespace CleanTemplate.Application.Test.Todos.Queries
{
    public class GetAllTodosQueryTest
    {
        [Fact]
        public async Task Handle_WithNoTodos_ReturnsEmpty()
        {
            var contextFactory = new ApplicationDbContextFactory();
            var context = await contextFactory.Create();
            var handler = new GetAllTodosQuery.GetAllTodosQueryHandler(context);

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
            var handler = new GetAllTodosQuery.GetAllTodosQueryHandler(context);

            var actual = await handler.Handle(new GetAllTodosQuery(), CancellationToken.None);

            Assert.NotNull(actual);
            Assert.Equal(TodoSeeder.DefaultTodoItems.Count,  actual.Todos.Count());
            Assert.Contains(actual.Todos, t => TodoSeeder.DefaultTodoItems[0].Id ==  t.Id);
            Assert.Contains(actual.Todos, t => TodoSeeder.DefaultTodoItems[0].Description ==  t.Description);
            Assert.Contains(actual.Todos, t => TodoSeeder.DefaultTodoItems[1].Id ==  t.Id);
            Assert.Contains(actual.Todos, t => TodoSeeder.DefaultTodoItems[1].Description ==  t.Description);
        }
    }
}
