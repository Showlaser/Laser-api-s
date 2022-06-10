using Microsoft.AspNetCore.Mvc.Testing;

namespace Vote_API.Tests.IntegrationTests.Factories
{
    internal class AuthFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
        }
    }
}
