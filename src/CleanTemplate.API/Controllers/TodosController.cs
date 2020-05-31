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
        [HttpPost]
        public async Task<ActionResult<long>> CreateTodo(CreateTodoListCommand listCommand)
        {
            var result = await Mediator.Send(listCommand);
            return CreatedAtAction(nameof(GetById), new { id = result }, result);
        }

        [HttpGet]
        public async Task<ActionResult<TodoListIndexResponse>> GetAll()
        {
            var result = await Mediator.Send(new GetTodoListIndexQuery());
            return new TodoListIndexResponse { Todos = result };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoListVm>> GetById(int id)
        {
            var result = await Mediator.Send(new GetTodoListQuery { Id = id });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoListVm>> Delete(int id)
        {
            await Mediator.Send(new DeleteTodoListCommand { Id = id });
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateTodoListCommand todoListCommand)
        {
            await Mediator.Send(todoListCommand);
            return NoContent();
        }

        [HttpPost("{id}/Item/Add")]
        public async Task<ActionResult<int>> AddTodoItem(int id, AddTodoItemCommand command)
        {
            var itemId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetTodoItem), new { id, itemId }, itemId);
        }

        [HttpGet("{id}/Item/{itemId}")]
        public async Task<ActionResult<TodoItemVm>> GetTodoItem(int id, int itemId)
        {
            var result = await Mediator.Send(new GetTodoItemByIdQuery { Id = id, ItemId = itemId });
            return Ok(result);
        }

        [HttpPut("{id}/Item/{itemId}")]
        public async Task<ActionResult<TodoItemVm>> UpdateTodoItem(int id, UpdateTodoItemCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}/Item/{itemId}/Delete")]
        public async Task<ActionResult> DeleteTodoItem(int id, int itemId)
        {
            await Mediator.Send(new DeleteTodoItemCommand { Id = id, ItemId = itemId });
            return NoContent();
        }
    }
}
