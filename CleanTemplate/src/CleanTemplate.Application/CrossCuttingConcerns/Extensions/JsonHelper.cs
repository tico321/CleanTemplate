using Newtonsoft.Json;

namespace CleanTemplate.Application.CrossCuttingConcerns.Extensions
{
    public static class JsonHelper
    {
        public static string ToJson(this object obj, bool pretty = false)
        {
            var format = pretty ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(
                obj,
                // pretty
                Formatting.Indented,
                // to prevent issues with circular references
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
        }
    }
}