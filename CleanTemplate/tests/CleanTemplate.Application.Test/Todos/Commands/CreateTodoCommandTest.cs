using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Commands.Create;
using FluentValidation.TestHelper;
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
            var command = new CreateTodoCommand {Description = "Description"};
            var handler = new CreateTodoCommand.CreateTodoHandler(context);

            var id = await handler.Handle(command, CancellationToken.None);
            var actual = context.TodoItems.Find(id);

            Assert.NotNull(actual);
            Assert.Equal(command.Description, actual.Description);
        }

        [Fact]
        public void Validator()
        {
            var validator = new CreateTodoCommandValidator();

            validator
                .ShouldHaveValidationErrorFor(c => c.Description, null as string)
                .WithErrorMessage("Description cannot be null");
            validator
                .ShouldHaveValidationErrorFor(c => c.Description, "123")
                .WithErrorMessage("5 is the minimum length");
            validator.ShouldNotHaveValidationErrorFor(c => c.Description, "123456");
        }
    }
}
