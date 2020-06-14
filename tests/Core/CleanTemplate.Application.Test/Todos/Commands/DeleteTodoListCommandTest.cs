using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Commands.DeleteTodoList;
using Xunit;

namespace CleanTemplate.Application.Test.Todos.Commands
{
    public class DeleteTodoListCommandTest : IClassFixture<RequestTestFixture>
    {
        public DeleteTodoListCommandTest(RequestTestFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly RequestTestFixture _fixture;

        [Fact]
        public async Task Handler_DeletesTodoList()
        {
            await TodoSeeder.GetSeeder(TodoSeeder.DefaultTodoLists)(_fixture.Context);
            var target = _fixture.Context.TodoLists.First();
            var command = new DeleteTodoListCommand { Id = target.Id };
            var handler = new DeleteTodoListCommand.Handler(_fixture.Context);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            Assert.Null(_fixture.Context.TodoLists.Find(target.Id));
        }

        [Fact]
        public async Task Handler_WhenTheListDoesntExist_ThrowsNotFoundException()
        {
            var command = new DeleteTodoListCommand { Id = -1 };
            var handler = new DeleteTodoListCommand.Handler(_fixture.Context);

            try
            {
                var result = await handler.Handle(command, CancellationToken.None);
                Assert.False(condition: true, "Should throw not found exception");
            }
            catch (NotFoundException e)
            {
                Assert.Contains("-1", e.Key);
            }
        }
    }
}
