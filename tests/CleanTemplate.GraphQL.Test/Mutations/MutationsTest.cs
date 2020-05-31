using System.Net;
using System.Threading.Tasks;
using CleanTemplate.GraphQL.Test.TestHelpers;
using Xunit;

namespace CleanTemplate.GraphQL.Test.Mutations
{
    public class MutationsTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public MutationsTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateTodoList_ThenUpdateIt_ThenDeleteIt()
        {
            var client = _factory.CreateClient();
            // Create Fail
            var query =
                @"mutation {
                  createTodoList(command: {
                    description: """"
                  })
                }";
            var queryContent = ApiTestHelper.GetQueryContent(query);
            var response = await client.PostAsync("/", queryContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var todoResponse = await ApiTestHelper.GetJsonObject(response);
            Assert.Contains("One or more validation failures have occurred.", todoResponse["errors"].ToString());
            Assert.Contains("5 is the minimum length", todoResponse["errors"].ToString());
            // Create TodoList
            query =
                @"mutation {
                  createTodoList(command: {
                    description: ""home list""
                  })
                }";
            queryContent = ApiTestHelper.GetQueryContent(query);
            response = await client.PostAsync("/", queryContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            todoResponse = await ApiTestHelper.GetJsonObject(response);
            var createdId = todoResponse["data"]["createTodoList"].ToObject<int>();

            // Update TodoList
            query =
                @"mutation {
                  updateTodoList(command: {
                    id: 1
                    description: ""new description""
                    displayOrder: 2
                  })
                }";
            queryContent = ApiTestHelper.GetQueryContent(query);
            response = await client.PostAsync("/", queryContent);
            todoResponse = await ApiTestHelper.GetJsonObject(response);
            Assert.True(todoResponse["data"]["updateTodoList"].ToObject<bool>());

            // Delete TodoList
            query =
                @"mutation {
                  deleteTodoList(id: 1)
                }";
            queryContent = ApiTestHelper.GetQueryContent(query);
            response = await client.PostAsync("/", queryContent);
            todoResponse = await ApiTestHelper.GetJsonObject(response);
            Assert.True(todoResponse["data"]["deleteTodoList"].ToObject<bool>());
        }

        [Fact]
        public async Task CreateTodoList_AddItem_ThenUpdateItem_ThenDeleteItem()
        {
            var client = _factory.CreateClient();
            // Create Item Fail
            var query =
                @"mutation {
                  createTodoItem(command: {
                    description: """"
                    todoListId: 123
                  })
                }";
            var queryContent = ApiTestHelper.GetQueryContent(query);
            var response = await client.PostAsync("/", queryContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var todoResponse = await ApiTestHelper.GetJsonObject(response);
            Assert.False(string.IsNullOrEmpty(todoResponse["errors"].ToString()));

            // Create TodoList
            query =
                @"mutation {
                  createTodoList(command: {
                    description: ""work list""
                  })
                }";
            queryContent = ApiTestHelper.GetQueryContent(query);
            response = await client.PostAsync("/", queryContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            todoResponse = await ApiTestHelper.GetJsonObject(response);
            var createdListId = todoResponse["data"]["createTodoList"].ToObject<int>();

            // Add TodoItem
            query = "mutation { createTodoItem(command: { description: \"todo 1\" todoListId:" + createdListId + " }) }";
            queryContent = ApiTestHelper.GetQueryContent(query);
            response = await client.PostAsync("/", queryContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            todoResponse = await ApiTestHelper.GetJsonObject(response);
            var createdId = todoResponse["data"]["createTodoItem"].ToObject<int>();

            // Update TodoList
            query =
                @"mutation {
                  updateTodoItem(command: {
                    id: " + createdListId +
                    $"itemId: {createdId}" + @"
                    description: ""new description""
                    displayOrder: 2
                    state: ""Pending""
                  })
                }";
            queryContent = ApiTestHelper.GetQueryContent(query);
            response = await client.PostAsync("/", queryContent);
            todoResponse = await ApiTestHelper.GetJsonObject(response);
            Assert.True(todoResponse["data"]["updateTodoItem"].ToObject<bool>());

            // Delete TodoList
            query = "mutation { deleteTodoItem(id: " + createdListId + " itemId: " + createdId + ") }";
            queryContent = ApiTestHelper.GetQueryContent(query);
            response = await client.PostAsync("/", queryContent);
            todoResponse = await ApiTestHelper.GetJsonObject(response);
            Assert.True(todoResponse["data"]["deleteTodoItem"].ToObject<bool>());
        }
    }
}
