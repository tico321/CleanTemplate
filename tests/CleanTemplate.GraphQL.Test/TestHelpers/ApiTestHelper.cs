using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CleanTemplate.GraphQL.Test.TestHelpers
{
    public class ApiTestHelper
    {
        public static StringContent GetRequestContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        public static StringContent GetQueryContent(string query)
        {
            return GetRequestContent(new Query { query = query });
        }

        public static async Task<JObject> GetJsonObject(HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();
            return JObject.Parse(stringResponse);
        }
    }

    public class Query
    {
        public string query { get; set; }
        public object variables { get; set; }
    }
}
