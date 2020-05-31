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
    public class DeleteTodoItemCommandTest : IClassFixture<MappingTestFixture>
    {
        public DeleteTodoItemCommandTest(MappingTestFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly MappingTestFixture _fixture;

        [Fact]
        public async Task Handle_RemovesTheItem()
        {
            var factory = new ApplicationDbContextFactory();
            var description = nameof(Handle_RemovesTheItem);
            var todoListWithItem =
                new TodoList("userId", description, displayOrder: 1).SequenceAddTodo(description);
            var todos = new List<TodoList> {todoListWithItem};
            var context = await factory.Create(TodoSeeder.GetSeeder(todos));
            var todoList = context.TodoLists.First(t => t.Description == description);
            var todoItem = todoList.Todos.First();
            var command = new DeleteTodoItemCommand {Id = todoList.Id, ItemId = todoItem.Id};
            var handler = new DeleteTodoItemCommand.Handler(context);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            var actualTodoList = context.TodoLists.First(t => t.Id == todoList.Id);
            Assert.True(actualTodoList.Todos.All(t => t.Id != todoItem.Id));
        }

        [Fact]
        public async Task Handle_WhenNotFound_ThrowsNotFoundException()
        {
            var factory = new ApplicationDbContextFactory();
            var seeder = TodoSeeder.GetSeeder(TodoSeeder.DefaultTodoLists);
            var context = await factory.Create(seeder);
            await TodoSeeder.GetSeeder(TodoSeeder.DefaultTodoLists)(context);
            var command = new DeleteTodoItemCommand {Id = -1, ItemId = -2};
            var handler = new DeleteTodoItemCommand.Handler(context);

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
            var existingTodoList = context.TodoLists.First();
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
