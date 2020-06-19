using System.Threading.Tasks;
using CleanTemplate.Application.Todos.Commands.AddTodoItem;
using CleanTemplate.Application.Todos.Commands.CreateTodoList;
using CleanTemplate.Application.Todos.Commands.DeleteTodoItem;
using CleanTemplate.Application.Todos.Commands.DeleteTodoList;
using CleanTemplate.Application.Todos.Commands.UpdateTodoItem;
using CleanTemplate.Application.Todos.Commands.UpdateTodoList;
using CleanTemplate.Application.Todos.Queries.GetTodoItemById;
using CleanTemplate.Application.Todos.Queries.GetTodoListById;
using CleanTemplate.Application.Todos.Queries.GetTodoListIndex;
using Microsoft.AspNetCore.Mvc;

namespace CleanTemplate.API.Controllers
{
    [ApiController]
    public class TodosController : BaseApiController
    {
        /// <summary>
        ///     Create a TodoList.
        /// </summary>
        /// <param name="listCommand">Object that contains the information of the TodoList.</param>
        /// <returns>The id of the created TodoList.</returns>
        [HttpPost]
        public async Task<ActionResult<long>> CreateTodo(CreateTodoListCommand listCommand)
        {
            var result = await Mediator.Send(listCommand);
            return CreatedAtAction(nameof(GetById), new { id = result }, result);
        }

        /// <summary>
        ///     Returns an Index of all the TodoLists with basic information of each one.
        /// </summary>
        /// <returns>All the TodoLists.</returns>
        [HttpGet]
        public async Task<ActionResult<TodoListIndexResponse>> GetAll()
        {
            var result = await Mediator.Send(new GetTodoListIndexQuery());
            return new TodoListIndexResponse { Todos = result };
        }

        /// <summary>
        ///     Get a TodoList by the Id
        /// </summary>
        /// <param name="id">The id of the TodoList</param>
        /// <returns>A ToList that includes its TodoItems.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoListVm>> GetById(int id)
        {
            var result = await Mediator.Send(new GetTodoListQuery { Id = id });
            return Ok(result);
        }

        /// <summary>
        ///     Deletes a TodoList including all its TodoItems.
        /// </summary>
        /// <param name="id">The id of the TodoList.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteTodoListCommand { Id = id });
            return NoContent();
        }

        /// <summary>
        ///     Updates a TodoList.
        /// </summary>
        /// <param name="id">The id of the TodoList.</param>
        /// <param name="todoListCommand">Updated information of the TodoList.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateTodoListCommand todoListCommand)
        {
            await Mediator.Send(todoListCommand);
            return NoContent();
        }

        /// <summary>
        ///     Adds an Item to a TodoList.
        /// </summary>
        /// <param name="id">The id of the target TodoList.</param>
        /// <param name="command">Information about the TodoItem.</param>
        /// <returns>The id of the created TodoItem.</returns>
        [HttpPost("{id}/Item/Add")]
        public async Task<ActionResult<int>> AddTodoItem(int id, AddTodoItemCommand command)
        {
            var itemId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetTodoItem), new { id, itemId }, itemId);
        }

        /// <summary>
        ///     Gets a TodoItem with all of its information.
        /// </summary>
        /// <param name="id">The id of the TodoList.</param>
        /// <param name="itemId">The id of the TodoItem.</param>
        /// <returns>A TodoItem.</returns>
        [HttpGet("{id}/Item/{itemId}")]
        public async Task<ActionResult<TodoItemVm>> GetTodoItem(int id, int itemId)
        {
            var result = await Mediator.Send(new GetTodoItemByIdQuery { Id = id, ItemId = itemId });
            return Ok(result);
        }

        /// <summary>
        ///     Updates a TodoItem.
        /// </summary>
        /// <param name="id">The id of the TodoItem.</param>
        /// <param name="command">Updated information of the TodoItem.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}/Item/{itemId}")]
        public async Task<ActionResult> UpdateTodoItem(int id, UpdateTodoItemCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        ///     Deletes a TodoItem.
        /// </summary>
        /// <param name="id">The id of the TodoList that contains the TodoItem.</param>
        /// <param name="itemId">The id of the TodoItem.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}/Item/{itemId}/Delete")]
        public async Task<ActionResult> DeleteTodoItem(int id, int itemId)
        {
            await Mediator.Send(new DeleteTodoItemCommand { Id = id, ItemId = itemId });
            return NoContent();
        }
    }
}
