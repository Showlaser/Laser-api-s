using Auth_API.Controllers;
using Auth_API.Models.FromFrontend.User;
using Auth_API.Models.Helper;
using Auth_API.Tests.UnitTests;
using Auth_API.Tests.UnitTests.MockedLogics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Auth_API.Tests.ComponentTests
{
    /// <summary>
    /// Exercises the UserController together with the real UserLogic, the result helper and a
    /// mocked data access layer. This validates request -> logic -> response wiring, including
    /// the authentication cookies that are written to the HTTP response.
    /// </summary>
    [TestClass]
    public class UserControllerTest
    {
        private static UserController BuildController()
        {
            TestHelper.SetupTestEnvironment();
            UserController controller = new(new MockedUserLogic().UserLogic, new ControllerResultHelper());

            DefaultHttpContext httpContext = new();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            return controller;
        }

        [TestMethod]
        public async Task LoginWithValidCredentialsSetsAuthCookiesTest()
        {
            UserController controller = BuildController();

            ActionResult? result = await controller.Login(new User { Username = "TestUser", Password = "qwerty" });

            Assert.IsInstanceOfType(result, typeof(OkResult));
            string setCookies = controller.Response.Headers["Set-Cookie"].ToString();
            StringAssert.Contains(setCookies, "jwt=");
            StringAssert.Contains(setCookies, "refreshToken=");
        }

        [TestMethod]
        public async Task LoginWithWrongPasswordReturnsUnauthorizedTest()
        {
            UserController controller = BuildController();

            ActionResult? result = await controller.Login(new User { Username = "TestUser", Password = "wrongPassword" });

            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public async Task LoginUnknownUserReturnsUnauthorizedTest()
        {
            UserController controller = BuildController();

            ActionResult? result = await controller.Login(new User { Username = "ghost", Password = "qwerty" });

            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void LogoutReturnsOkAndWritesExpiredCookiesTest()
        {
            UserController controller = BuildController();

            ActionResult result = controller.Logout();

            Assert.IsInstanceOfType(result, typeof(OkResult));
            string setCookies = controller.Response.Headers["Set-Cookie"].ToString();
            StringAssert.Contains(setCookies, "jwt=");
            StringAssert.Contains(setCookies, "refreshToken=");
        }
    }
}
