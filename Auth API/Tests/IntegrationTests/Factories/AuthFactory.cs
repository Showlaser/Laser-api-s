using Microsoft.AspNetCore.Mvc.Testing;

namespace Auth_API.Tests.IntegrationTests.Factories
{
    internal class AuthFactory : WebApplicationFactory<Program>
    {
        private readonly DbFixture _dbFixture;

        public AuthFactory(DbFixture dbFixture)
            => _dbFixture = dbFixture;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
        }
    }
}
