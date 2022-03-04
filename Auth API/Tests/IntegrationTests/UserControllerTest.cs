using Auth_API.Dal;
using Auth_API.Models.FromFrontend.User;
using Auth_API.Tests.IntegrationTests.Factories;
using Auth_API.Tests.IntegrationTests.TestModels;
using Auth_API.Tests.UnitTests;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using NUnit.Framework;
using System.Net;

namespace Auth_API.Tests.IntegrationTests
{
    [TestFixture]
    internal class UserControllerTest
    {
        private AuthFactory _factory;
        private readonly CookieContainerHandler _handler = new();

        [SetUp]
        public void Setup()
        {
            DbFixture fixture = new();
            _factory = new AuthFactory(fixture);
            TestHelper.SetEnvironmentVariables();
            AddTestUser().Wait();
            GetAuthorizationTokens().Wait();
        }

        private async Task AddTestUser()
        {
            HttpClient client = _factory.CreateDefaultClient();
            User user = new TestUser().User;

            FormUrlEncodedContent formContent = new(new[]
            {
                new KeyValuePair<string, string>("username", user.UserName),
                new KeyValuePair<string, string>("password", user.Password)
            });

            await client.PostAsync("user", formContent);
        }

        private async Task<Guid> GetTestUserUuid()
        {
            User user = new TestUser().User;
            UserDal userDal = _factory.Services.GetService<UserDal>();
            return (await userDal.Find(user.UserName)).Uuid;
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

        [Test]
        public async Task UpdateUserTest()
        {
            try
            {
                HttpClient client = _factory.CreateDefaultClient(_handler);
                User user = new TestUser().User;
                HttpResponseMessage putResponse = await client.PutAsync("user", new JsonContent<User>(user));
                putResponse.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
