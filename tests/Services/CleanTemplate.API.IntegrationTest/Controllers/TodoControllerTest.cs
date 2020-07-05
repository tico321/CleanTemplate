using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CleanTemplate.API.TestHelpers;
using CleanTemplate.Application.Test.Todos;
using CleanTemplate.Application.Todos.Commands.AddTodoItem;
using CleanTemplate.Application.Todos.Commands.CreateTodoList;
using CleanTemplate.Application.Todos.Commands.UpdateTodoItem;
using CleanTemplate.Application.Todos.Commands.UpdateTodoList;
using CleanTemplate.Application.Todos.Model;
using CleanTemplate.Application.Todos.Queries.GetTodoListById;
using CleanTemplate.Application.Todos.Queries.GetTodoListIndex;
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
        public async Task AddTodoListItem_Fail()
        {
            var client = _factory.CreateClient();
            var command = new AddTodoItemCommand { TodoListId = -1, Description = nameof(TodoControllerTest) };
            var request = ApiTestHelper.GetRequestContent(command);

            var response = await client.PostAsync("/api/Todos/1/Item", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AddTodoListItem_RetrieveIt_UpdateIt_ThenRemoveIt()
        {
            // First we create a todo item
            var client = _factory.CreateClient();
            var command = new AddTodoItemCommand { TodoListId = 1, Description = nameof(TodoControllerTest) };
            var request = ApiTestHelper.GetRequestContent(command);

            var response = await client.PostAsync("/api/Todos/1/Item", request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdResponse = await ApiTestHelper.GetResponseContent<int>(response);
            var createdId = createdResponse.Result;

            // Retrieve the item
            response = await client.GetAsync($"/api/Todos/1/Item/{createdId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Update the item
            var updateCommand = new UpdateTodoItemCommand
            {
                Id = 1,
                ItemId = createdId,
                Description = "new desc",
                State = TodoItemState.Completed.Name,
                DisplayOrder = 1
            };
            response = await client.PutAsync(
                $"/api/Todos/1/Item/{createdId}",
                ApiTestHelper.GetRequestContent(updateCommand));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Finally we remove the item
            response = await client.DeleteAsync($"/api/Todos/1/Item/{createdId}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task CreatesATodoList_RetrieveIt_UpdateIt_ThenRemoveIt()
        {
            // Create the list
            var command = new CreateTodoListCommand { Description = "description" };
            var client = _factory.CreateClient();
            var request = ApiTestHelper.GetRequestContent(command);

            var response = await client.PostAsync("/api/Todos", request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            // Retrieve the list
            var id = (await ApiTestHelper.GetResponseContent<int>(response)).Result;
            response = await client.GetAsync("/api/Todos");
            var todoList = await ApiTestHelper.GetResponseContent<TodoListIndexResponse>(response);
            Assert.Contains(todoList.Result.Todos, t => t.Id == id);

            // Update the list
            var updateCommand = new UpdateTodoListCommand { Id = id, Description = "new description", DisplayOrder = 10 };
            response = await client.PutAsync($"/api/Todos/{id}", ApiTestHelper.GetRequestContent(updateCommand));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Delete the list
            response = await client.DeleteAsync($"/api/Todos/{id}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task CreatesATodoLists_Fail()
        {
            var command = new CreateTodoListCommand { Description = null };
            var client = _factory.CreateClient();
            var request = ApiTestHelper.GetRequestContent(command);

            var response = await client.PostAsync("/api/Todos", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var apiResponse = await ApiTestHelper.GetResponseContent<long>(response);
            Assert.True(apiResponse.IsError);
            Assert.Equal(
                "Description cannot be null",
                apiResponse.ResponseException?.ExceptionMessage?.ProblemDetails["Description"].First());
        }

        [Fact]
        public async Task GetTodoListById()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/Todos/2");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var todoList = await ApiTestHelper.GetResponseContent<TodoListVm>(response);
            Assert.NotNull(todoList.Result);
            Assert.Equal(TodoSeeder.DefaultTodoLists[1].Todos.Count(), todoList.Result.Count);
            Assert.Equal(TodoSeeder.DefaultTodoLists[1].Todos.Count(), todoList.Result.Todos.Count());

            // Returns 401 when it doesn't exists
            response = await client.GetAsync("/api/Todos/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetTodoListIndex()
        {
            var client = _factory.Reset().CreateClient();

            var response = await client.GetAsync("/api/Todos");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var todoList = await ApiTestHelper.GetResponseContent<TodoListIndexResponse>(response);
            Assert.NotNull(todoList.Result);
            Assert.Contains(
                todoList.Result.Todos,
                t => t.Description == TodoSeeder.DefaultTodoLists[0].Description);
            Assert.Contains(
                todoList.Result.Todos,
                t => t.Description == TodoSeeder.DefaultTodoLists[1].Description);
        }
    }
}
