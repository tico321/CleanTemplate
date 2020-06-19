using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Queries.GetTodoListById;
using Xunit;

namespace CleanTemplate.Application.Test.Todos.Queries
{
    public class GetTodoListByIdQueryTest : IClassFixture<MappingTestFixture>
    {
        public GetTodoListByIdQueryTest(MappingTestFixture mapping)
        {
            _mapping = mapping;
        }

        private readonly MappingTestFixture _mapping;

        [Fact]
        public async Task Get_WithExistingList()
        {
            var factory = new ApplicationDbContextFactory();
            var context = await factory.Create(TodoSeeder.GetSeeder(TodoSeeder.DefaultTodoLists));
            var target = context.TodoLists.First(t => t.Todos.Count() >= 2);
            var query = new GetTodoListQuery { Id = target.Id };
            var handler = new GetTodoListQuery.Handler(context, _mapping.Mapper);

            var actual = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(actual);
            for (var i = 1; i < actual.Todos.Count; i++)
            {
                var current = actual.Todos[i];
                var prev = actual.Todos[i - 1];
                Assert.True(current.DisplayOrder > prev.DisplayOrder);
            }
        }

        [Fact]
        public async Task Get_WithNonExistingList()
        {
            var factory = new ApplicationDbContextFactory();
            var context = await factory.Create(TodoSeeder.GetSeeder(TodoSeeder.DefaultTodoLists));
            var query = new GetTodoListQuery { Id = 100 };
            var handler = new GetTodoListQuery.Handler(context, _mapping.Mapper);

            try
            {
                await handler.Handle(query, CancellationToken.None);
                Assert.True(false, "Should throw not found exception");
            }
            catch (NotFoundException e)
            {
                Assert.NotNull(e);
                Assert.Contains("100", e.Message);
            }
        }
    }
}
