using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Commands.UpdateTodoList;
using FluentValidation.TestHelper;
using Xunit;

namespace CleanTemplate.Application.Test.Todos.Commands
{
    public class UpdateTodoListCommandTest : IClassFixture<RequestTestFixture>
    {
        public UpdateTodoListCommandTest(RequestTestFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly RequestTestFixture _fixture;

        [Fact]
        public async Task Handler()
        {
            await TodoSeeder.GetSeeder(TodoSeeder.DefaultTodoLists)(_fixture.Context);
            var target = _fixture.Context.TodoLists.First();
            var command = new UpdateTodoListCommand { Id = target.Id, Description = "new description", DisplayOrder = 23 };
            var handler = new UpdateTodoListCommand.Handler(_fixture.Context, _fixture.Mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            var actual = await _fixture.Context.TodoLists.FindAsync(target.Id);
            Assert.Equal(command.Description, actual.Description);
            Assert.Equal(command.DisplayOrder, actual.DisplayOrder);
        }

        [Fact]
        public async Task Handler_WhenTheTodoDoesNotExist_ThrowsException()
        {
            var command = new UpdateTodoListCommand { Id = 123, Description = "new description", DisplayOrder = 23 };
            var handler = new UpdateTodoListCommand.Handler(_fixture.Context, _fixture.Mapper);

            try
            {
                await handler.Handle(command, CancellationToken.None);
                Assert.True(false, "should throw NotFoundException");
            }
            catch (NotFoundException e)
            {
                Assert.Contains("123", e.Key);
            }
        }

        [Fact]
        public void Validator()
        {
            var validator = new UpdateTodoListCommand.Validator();
            validator.ShouldHaveValidationErrorFor(c => c.Description, null as string);
            validator.ShouldHaveValidationErrorFor(c => c.Description, "");
            validator.ShouldHaveValidationErrorFor(c => c.Description, new string('*', 5000));
            validator.ShouldNotHaveValidationErrorFor(c => c.Description, "123456");
        }
    }
}
