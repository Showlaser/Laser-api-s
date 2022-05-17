using Auth_API.Models.FromFrontend.User;
using Auth_API.Tests.IntegrationTests.Factories;
using Auth_API.Tests.IntegrationTests.TestModels;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.User;

namespace Auth_API.Tests.IntegrationTests
{
    [TestClass]
    public class UserControllerTest
    {
        private readonly AuthFactory _factory = new();
        private readonly CookieContainerHandler _handler = new();
        private IUserDal _userDal;

        [TestInitialize]
        public void Setup()
        {
            TestHelper.SetupTestEnvironment();
            _userDal = TestHelper.Services.GetService<IUserDal>();
            CheckIfTestUserExists().Wait();
            GetAuthorizationTokens().Wait();
        }

        private async Task CheckIfTestUserExists()
        {
            UserDto? testUser = await _userDal.Find(new TestUser().User.Username);
            if (testUser == null)
            {
                throw new KeyNotFoundException("No test user found! " +
                                               "Create a test user in the database first! " +
                                               "Make sure to remove the user from the disabled user table and user activation table!");
            }
        }

        private async Task GetAuthorizationTokens()
        {
            CookieContainer cookieContainer = new();
            CookieContainerHandler handler = new(cookieContainer);

            HttpClient client = _factory.CreateDefaultClient(handler);
            User user = new TestUser().User;
            
            var response = await client.PostAsync("/user/login", new JsonContent<User>(user));
            response.EnsureSuccessStatusCode();
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
            user.Username = "newtestuser";

            HttpResponseMessage putResponse = await client.PutAsync("user", new JsonContent<User>(user));
            putResponse.EnsureSuccessStatusCode();

            user.Username = new TestUser().User.Username;
            HttpResponseMessage restoreUsernameResponse = await client.PutAsync("user", new JsonContent<User>(user));
            if (restoreUsernameResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Failed to restore the test user username, set it to TestUser in the database.");
            }
        }
    }
}
