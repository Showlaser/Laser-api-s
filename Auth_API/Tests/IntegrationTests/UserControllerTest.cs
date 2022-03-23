using Auth_API.Models.FromFrontend.User;
using Auth_API.Tests.IntegrationTests.Factories;
using Auth_API.Tests.IntegrationTests.TestModels;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Auth_API.Tests.IntegrationTests
{
    [TestClass]
    public class UserControllerTest
    {
        private readonly AuthFactory _factory = new();
        private readonly CookieContainerHandler _handler = new();

        [TestInitialize]
        public void Setup()
        {
            TestHelper.SetEnvironmentVariables();
            AddTestUser().Wait();
            GetAuthorizationTokens().Wait();
        }

        private async Task AddTestUser()
        {
            HttpClient client = _factory.CreateDefaultClient();
            User user = new TestUser().User;

            await client.PostAsync("user", new JsonContent<User>(user));
        }

        private async Task GetAuthorizationTokens()
        {
            CookieContainer cookieContainer = new();
            CookieContainerHandler handler = new(cookieContainer);

            HttpClient client = _factory.CreateDefaultClient(handler);
            User user = new TestUser().User;

            FormUrlEncodedContent formContent = new(new[]
            {
                new KeyValuePair<string, string>("username", user.UserName),
                new KeyValuePair<string, string>("password", user.Password)
            });

            await client.PostAsync("/user/login", formContent);
            foreach (Cookie cookie in cookieContainer.GetAllCookies())
            {
                _handler.Container.Add(cookie);
            }
        }

        [TestMethod]
        public async Task UpdateUserTest()
        {
            HttpClient client = _factory.CreateDefaultClient(_handler);
            User user = new TestUser().User;
            HttpResponseMessage putResponse = await client.PutAsync("user", new JsonContent<User>(user));
            putResponse.EnsureSuccessStatusCode();
        }
    }
}
