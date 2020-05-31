using System.Net;
using System.Threading.Tasks;
using CleanTemplate.GraphQL.Test.TestHelpers;
using Xunit;

namespace CleanTemplate.GraphQL.Test.Queries
{
    public class QueriesTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public QueriesTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly CustomWebApplicationFactory<Startup> _factory;

        [Fact]
        public async Task GetTodoListById()
        {
            var client = _factory.Reset().CreateClient();

            var query =
                @"{
                  todoListById(id: 1) {
                    description
                    displayOrder
                    count
                    todos {
                      description
                      displayOrder
                      state
                    }
                  }
                }";
            var queryContent = ApiTestHelper.GetQueryContent(query);
            var response = await client.PostAsync("/", queryContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonResponse = await ApiTestHelper.GetJsonObject(response);
            var todoList = jsonResponse["data"]["todoListById"];
            var errors = jsonResponse["errors"];
            Assert.NotNull(todoList);
            Assert.Null(errors);

            // With an invalid id
            query =
                @"{
                  todoListById(id: 123) {
                    description
                    displayOrder
                    count
                    todos {
                      description
                      displayOrder
                      state
                    }
                  }
                }";
            queryContent = ApiTestHelper.GetQueryContent(query);
            response = await client.PostAsync("/", queryContent);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            jsonResponse = await ApiTestHelper.GetJsonObject(response);
            todoList = jsonResponse["data"]["todoListById"];
            errors = jsonResponse["errors"];
            Assert.NotNull(errors);
            Assert.Contains("123", errors.ToString());
        }

        [Fact]
        public async Task GetTodoListIndex()
        {
            var client = _factory.Reset().CreateClient();
            var query =
                @"{
                  todoLists(first: 10) {
                    nodes {
                      id
                      description
                      displayOrder
                      count
                    }
                  }
                }";
            var queryContent = ApiTestHelper.GetQueryContent(query);
            var response = await client.PostAsync("/", queryContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
