using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Auth_API.Tests.IntegrationTests
{
    public class JsonContent<T> : StringContent
    {
        public JsonContent(T entity)
            : base(JsonConvert.SerializeObject(entity))
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}
