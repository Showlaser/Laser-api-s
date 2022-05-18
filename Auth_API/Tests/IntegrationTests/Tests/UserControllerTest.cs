using Auth_API.Models.FromFrontend.User;
using Auth_API.Tests.IntegrationTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Auth_API.Tests.IntegrationTests.Tests
{
    [TestClass]
    public class UserControllerTest
    {
        [TestInitialize]
        public void Setup()
        {
            TestHelper.SetupTestEnvironment();
        }

        [TestMethod]
        public async Task UpdateUserTest()
        {
            User user = new TestUser().User;
            user.Username = "newtestuser";

            HttpResponseMessage putResponse = await TestHelper.Client.PutAsync("user", new JsonContent<User>(user));
            putResponse.EnsureSuccessStatusCode();

            user.Username = new TestUser().User.Username;
            HttpResponseMessage restoreUsernameResponse = await TestHelper.Client.PutAsync("user", new JsonContent<User>(user));
            if (restoreUsernameResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Failed to restore the test user username, set it to TestUser in the database.");
            }
        }
    }
}
