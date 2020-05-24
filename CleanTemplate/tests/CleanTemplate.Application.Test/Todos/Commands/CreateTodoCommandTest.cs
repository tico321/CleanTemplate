using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Commands.Create;
using Xunit;

namespace CleanTemplate.Application.Test.Todos.Commands
{
    public class CreateTodoCommandTest
    {
        [Fact]
        public async Task Handle_PersistsTodoItem()
        {
            var dbFactory = new ApplicationDbContextFactory();
            using var context = await dbFactory.Create();
            var command = new CreateTodoCommand
            {
                Description = "Description"
            };
            var handler = new CreateTodoCommand.CreateTodoHandler(context);

            var id = await handler.Handle(command, CancellationToken.None);
            var actual = context.TodoItems.Find(id);

            Assert.NotNull(actual);
            Assert.Equal(command.Description, actual.Description);
        }
    }
}
