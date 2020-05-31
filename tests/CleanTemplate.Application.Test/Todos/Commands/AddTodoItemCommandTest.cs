using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Commands.AddTodoItem;
using Xunit;

namespace CleanTemplate.Application.Test.Todos.Commands
{
    public class AddTodoItemCommandTest
    {
        [Fact]
        public async Task Handler_AddsNewItem()
        {
            var contextFactory = new ApplicationDbContextFactory();
            var context = await contextFactory.Create(TodoSeeder.GetSeeder(TodoSeeder.DefaultTodoLists));
            var id = context.TodoLists.First().Id;
            var description = nameof(AddTodoItemCommand);
            var command = new AddTodoItemCommand { TodoListId = 1, Description = description };
            var handler = new AddTodoItemCommand.Handler(context);

            var itemId = await handler.Handle(command, CancellationToken.None);

            var todoList = context.TodoLists.Find(id);
            var addedItem = todoList.Todos.FirstOrDefault(t => t.Id == itemId);
            Assert.NotNull(addedItem);
            Assert.Equal(description, addedItem.Description);
            Assert.Equal(todoList, addedItem.TodoList);
            Assert.Equal(todoList.Id, addedItem.TodoListId);
        }

        [Fact]
        public async Task Validator()
        {
            var contextFactory = new ApplicationDbContextFactory();
            var context = await contextFactory.Create(TodoSeeder.GetSeeder(TodoSeeder.DefaultTodoLists));
            var description = nameof(AddTodoItemCommand);
            var command = new AddTodoItemCommand { TodoListId = 1, Description = description };
            var validator = new AddTodoItemCommand.Validator(context);

            var validation = validator.Validate(command);
            Assert.True(validation.IsValid);

            // With Invalid Id
            command.TodoListId = 10;
            validation = validator.Validate(command);
            Assert.False(validation.IsValid);
            Assert.Contains(validation.Errors, e => e.PropertyName == nameof(AddTodoItemCommand.TodoListId));

            // With Invalid Description
            command.TodoListId = 1;
            command.Description = null;
            validation = validator.Validate(command);
            Assert.False(validation.IsValid);
            Assert.Contains(validation.Errors, e => e.PropertyName == nameof(AddTodoItemCommand.Description));
        }
    }
}
