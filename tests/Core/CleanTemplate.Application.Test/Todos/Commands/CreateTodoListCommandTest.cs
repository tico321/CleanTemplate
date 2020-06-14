using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Commands.CreateTodoList;
using FluentValidation.TestHelper;
using Xunit;

namespace CleanTemplate.Application.Test.Todos.Commands
{
    public class CreateTodoListCommandTest
    {
        [Fact]
        public async Task Handle_PersistsTodoItem()
        {
            var dbFactory = new ApplicationDbContextFactory();
            using var context = await dbFactory.Create();
            var command = new CreateTodoListCommand { Description = "Description" };
            var handler = new CreateTodoListCommand.Handler(context, new FakeUserService());

            var id = await handler.Handle(command, CancellationToken.None);
            var actual = context.TodoLists.Find(id);

            Assert.NotNull(actual);
            Assert.Equal(command.Description, actual.Description);
        }

        [Fact]
        public void Validator()
        {
            var validator = new CreateTodoListCommand.Validator();

            validator
                .ShouldHaveValidationErrorFor(c => c.Description, null as string)
                .WithErrorMessage("Description cannot be null");
            validator
                .ShouldHaveValidationErrorFor(c => c.Description, "123")
                .WithErrorMessage("5 is the minimum length");
            validator
                .ShouldHaveValidationErrorFor(c => c.Description, new string(c: '*', count: 201))
                .WithErrorMessage("200 is the maximum length.");
            validator.ShouldNotHaveValidationErrorFor(c => c.Description, "123456");
        }
    }
}
