using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using Newtonsoft.Json;

namespace CleanTemplate.API.TestHelpers
{
    public class ApiTestHelper
    {
        public static StringContent GetRequestContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        public static async Task<APIResponse<T>> GetResponseContent<T>(HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<APIResponse<T>>(stringResponse);

            return result;
        }
    }

    public class APIResponse<T>
    {
        public string Message { get; set; }
        public T Result { get; set; }
        public bool IsError { get; set; }
        public APIError ResponseException { get; set; }
    }

    public class APIError
    {
        public ErrorDetails ExceptionMessage { get; set; }
    }
}
