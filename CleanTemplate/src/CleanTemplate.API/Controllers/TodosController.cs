using System.Threading.Tasks;
using CleanTemplate.Application.Todos.Commands.Create;
using CleanTemplate.Application.Todos.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace CleanTemplate.API.Controllers
{
    [ApiController]
    public class TodosController : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<long>> CreateTodo(CreateTodoCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        public async Task<ActionResult<TodoListVm>> GetAll()
        {
            return await Mediator.Send(new GetAllTodosQuery());
        }
    }
}