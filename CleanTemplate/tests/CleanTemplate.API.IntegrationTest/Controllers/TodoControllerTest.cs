using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CleanTemplate.API.TestHelpers;
using CleanTemplate.Application.Test.Todos;
using CleanTemplate.Application.Todos.Commands.Create;
using CleanTemplate.Application.Todos.Queries.GetAll;
using Xunit;

namespace CleanTemplate.API.Controllers
{
    public class TodoControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public TodoControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_WithTodos()
        {
            var client = _factory.Reset().CreateClient();

            var response = await client.GetAsync("/api/Todos");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var todoList = await ApiTestHelper.GetResponseContent<TodoListVm>(response);
            Assert.NotNull(todoList);
            Assert.Contains(todoList.Todos, t => t.Id == TodoSeeder.DefaultTodoItems[0].Id);
            Assert.Contains(todoList.Todos, t => t.Id == TodoSeeder.DefaultTodoItems[1].Id);
        }

        [Fact]
        public async Task CreatesATodo()
        {
            var command = new CreateTodoCommand{ Description = "description" };
            var client = _factory.CreateClient();
            var request = ApiTestHelper.GetRequestContent(command);

            var response = await client.PostAsync("/api/Todos", request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var id = await ApiTestHelper.GetResponseContent<long>(response);
            response = await client.GetAsync("/api/Todos");
            var todoList = await ApiTestHelper.GetResponseContent<TodoListVm>(response);
            Assert.True((bool) todoList.Todos.Any(t => t.Id == id));
        }
    }
}
