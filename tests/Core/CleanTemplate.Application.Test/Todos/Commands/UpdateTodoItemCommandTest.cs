using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Commands.UpdateTodoItem;
using CleanTemplate.Application.Todos.Model;
using CleanTemplate.Application.Todos.Queries.QueryObjects;
using FluentValidation.TestHelper;
using Xunit;

namespace CleanTemplate.Application.Test.Todos.Commands
{
    public class UpdateTodoItemCommandTest : IClassFixture<RequestTestFixture>
    {
        public UpdateTodoItemCommandTest(RequestTestFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly RequestTestFixture _fixture;

        [Fact]
        public async Task Handler()
        {
            await TodoSeeder.GetSeeder(TodoSeeder.DefaultTodoLists)(_fixture.Context);
            var targetList = _fixture.Context.TodoLists.First(t => t.Todos.Any());
            var target = targetList.Todos.First();
            var command = new UpdateTodoItemCommand
            {
                Id = targetList.Id,
                ItemId = target.Id,
                Description = "new description",
                DisplayOrder = 23,
                State = TodoItemState.Completed.Name
            };
            var handler = new UpdateTodoItemCommand.Handler(_fixture.Context, _fixture.Mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            var actual = await _fixture.Context.TodoLists.GetTodoItemById(
                targetList.Id,
                target.Id,
                CancellationToken.None);
            Assert.Equal(command.Description, actual.Description);
            Assert.Equal(command.DisplayOrder, actual.DisplayOrder);
            Assert.Equal(TodoItemState.Completed, actual.State);
        }

        [Fact]
        public async Task Handler_WhenTheItemIsNotFound_ThrowsNotFoundException()
        {
            var command = new UpdateTodoItemCommand
            {
                Id = 123,
                ItemId = 456,
                Description = "new description",
                DisplayOrder = 23,
                State = TodoItemState.Completed.Name
            };
            var handler = new UpdateTodoItemCommand.Handler(_fixture.Context, _fixture.Mapper);

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
            var validator = new UpdateTodoItemCommand.Validator();
            validator.ShouldHaveValidationErrorFor(c => c.Description, null as string);
            validator.ShouldHaveValidationErrorFor(c => c.Description, "");
            validator.ShouldHaveValidationErrorFor(c => c.Description, new string(c: '*', count: 5000));
            validator.ShouldNotHaveValidationErrorFor(c => c.Description, "123456");

            validator.ShouldHaveValidationErrorFor(c => c.State, "invalid");
            validator.ShouldNotHaveValidationErrorFor(c => c.State, TodoItemState.Completed.Name);
            validator.ShouldNotHaveValidationErrorFor(c => c.State, TodoItemState.Pending.Name);
        }
    }
}
