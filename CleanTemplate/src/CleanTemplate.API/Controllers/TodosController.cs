using System;
using System.Threading.Tasks;
using CleanTemplate.Application.Todos.Commands.Create;
using CleanTemplate.Application.Todos.Queries;
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
            var result = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new {id = result}, result);
        }

        [HttpGet]
        public async Task<ActionResult<TodoListVm>> GetAll()
        {
            return await Mediator.Send(new GetAllTodosQuery());
        }

        [HttpGet("{id}")]
        public Task<TodoVm> GetById(long id)
        {
            throw new NotImplementedException();
        }
    }
}
