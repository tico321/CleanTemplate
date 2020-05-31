using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Commands.DeleteTodoItem;
using CleanTemplate.Application.Todos.Model;
using Xunit;

namespace CleanTemplate.Application.Test.Todos.Commands
{
    public class DeleteTodoItemCommandTest : IClassFixture<RequestTestFixture>
    {
        public DeleteTodoItemCommandTest(RequestTestFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly RequestTestFixture _fixture;

        [Fact]
        public async Task Handle_RemovesTheItem()
        {
            var itemDescription = "deleteMe";
            var todoListWithItem = new TodoList("userId", "desc", displayOrder: 1).SequenceAddTodo(itemDescription);
            todoListWithItem.Id = 111; // because we are using the same context
            var todos = new List<TodoList> {todoListWithItem};
            await TodoSeeder.GetSeeder(todos)(_fixture.Context);
            var todoList = _fixture.Context.TodoLists.First(t => t.Todos.Any(t => t.Description == itemDescription));
            var todoItem = todoList.Todos.First();
            var command = new DeleteTodoItemCommand {Id = todoList.Id, ItemId = todoItem.Id};
            var handler = new DeleteTodoItemCommand.Handler(_fixture.Context);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            var actualTodoList = _fixture.Context.TodoLists.First(t => t.Id == todoList.Id);
            Assert.True(actualTodoList.Todos.All(t => t.Id != todoItem.Id));
        }

        [Fact]
        public async Task Handle_WhenNotFound_ThrowsNotFoundException()
        {
            await TodoSeeder.GetSeeder(TodoSeeder.DefaultTodoLists)(_fixture.Context);
            var command = new DeleteTodoItemCommand {Id = -1, ItemId = -2};
            var handler = new DeleteTodoItemCommand.Handler(_fixture.Context);

            // When the TodoListId doesn't exist
            try
            {
                var result = await handler.Handle(command, CancellationToken.None);
                Assert.True(condition: false, "It should throw NotFoundException");
            }
            catch (NotFoundException e)
            {
                Assert.Contains(command.Id.ToString(), e.Key);
            }

            // When the ItemId doesn't exist
            var existingTodoList = _fixture.Context.TodoLists.First();
            command.Id = existingTodoList.Id;
            try
            {
                var result = await handler.Handle(command, CancellationToken.None);
                Assert.True(condition: false, "It should throw NotFoundException");
            }
            catch (NotFoundException e)
            {
                Assert.Contains(command.ItemId.ToString(), e.Key);
            }
        }
    }
}
