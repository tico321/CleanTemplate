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
        public TodoControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly CustomWebApplicationFactory<Startup> _factory;

        [Fact]
        public async Task CreatesATodo()
        {
            var command = new CreateTodoCommand {Description = "description"};
            var client = _factory.CreateClient();
            var request = ApiTestHelper.GetRequestContent(command);

            var response = await client.PostAsync("/api/Todos", request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var id = (await ApiTestHelper.GetResponseContent<long>(response)).Result;
            response = await client.GetAsync("/api/Todos");
            var todoList = await ApiTestHelper.GetResponseContent<TodoListVm>(response);
            Assert.Contains(todoList.Result.Todos, t => t.Id == id);
        }

        [Fact]
        public async Task CreatesATodo_Fail()
        {
            var command = new CreateTodoCommand {Description = null};
            var client = _factory.CreateClient();
            var request = ApiTestHelper.GetRequestContent(command);

            var response = await client.PostAsync("/api/Todos", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var apiResponse = await ApiTestHelper.GetResponseContent<long>(response);
            Assert.True(apiResponse.IsError);
            Assert.Equal(
                "Description cannot be null",
                apiResponse?.ResponseException?.ExceptionMessage?.ProblemDetails["Description"].First());
        }

        [Fact]
        public async Task GetTodos()
        {
            var client = _factory.Reset().CreateClient();

            var response = await client.GetAsync("/api/Todos");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var todoList = await ApiTestHelper.GetResponseContent<TodoListVm>(response);
            Assert.NotNull(todoList.Result);
            Assert.Contains(todoList.Result.Todos, t => t.Id == TodoSeeder.DefaultTodoItems[index: 0].Id);
            Assert.Contains(todoList.Result.Todos, t => t.Id == TodoSeeder.DefaultTodoItems[index: 1].Id);
        }
    }
}
