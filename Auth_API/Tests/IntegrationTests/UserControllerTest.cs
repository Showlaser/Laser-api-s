using Auth_API.Models.FromFrontend.User;
using Auth_API.Tests.IntegrationTests.Factories;
using Auth_API.Tests.IntegrationTests.TestModels;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using Auth_API.Logic;

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
            
            var response = await client.PostAsync("/user/login", new JsonContent<User>(user));
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
            user.UserName = "newtestuser";

            HttpResponseMessage putResponse = await client.PutAsync("user", new JsonContent<User>(user));
            putResponse.EnsureSuccessStatusCode();
        }
    }
}
