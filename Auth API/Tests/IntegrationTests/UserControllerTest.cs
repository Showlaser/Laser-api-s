using Auth_API.Models.FromFrontend.User;
using Auth_API.Tests.IntegrationTests.Factories;
using Auth_API.Tests.IntegrationTests.TestModels;
using Auth_API.Tests.UnitTests;
using NUnit.Framework;

namespace Auth_API.Tests.IntegrationTests
{
    internal class UserControllerTest
    {
        private AuthFactory _factory;

        [SetUp]
        public void Setup()
        {
            DbFixture fixture = new();
            _factory = new AuthFactory(fixture);
            TestHelper.SetEnvironmentVariables();
            AddUser().Wait();
        }

        private async Task AddUser()
        {
            HttpClient client = _factory.CreateClient();
            User user = new TestUser().User;

            HttpResponseMessage postResponse = await client.PostAsync("/user", new JsonContent<User>(user));
            postResponse.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task AddUserTest()
        {
            HttpClient client = _factory.CreateClient();
            User user = new TestUser().User;

            HttpResponseMessage postResponse = await client.PostAsync("/user", new JsonContent<User>(user));
            postResponse.EnsureSuccessStatusCode();

        }
    }
}
