using Auth_API.CustomExceptions;
using Auth_API.Models.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Security;

namespace Auth_API.Tests.ComponentTests
{
    /// <summary>
    /// Verifies that the central exception handling translates domain exceptions into the
    /// correct HTTP status codes. This is the contract every controller relies on.
    /// </summary>
    [TestClass]
    public class ControllerResultHelperTest
    {
        private readonly ControllerResultHelper _helper = new();

        [TestMethod]
        public async Task SuccessfulVoidTaskReturnsOkTest()
        {
            ActionResult result = await _helper.Execute(Task.CompletedTask);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task SuccessfulTypedTaskReturnsValueTest()
        {
            ActionResult<string> result = await _helper.Execute(Task.FromResult("payload"));
            Assert.AreEqual("payload", result.Value);
        }

        [TestMethod]
        [DynamicData(nameof(ExceptionCases))]
        public async Task ExceptionMapsToExpectedStatusCodeTest(Exception exception, int expectedStatusCode)
        {
            ActionResult result = await _helper.Execute(Task.FromException(exception));

            int actualStatusCode = result is IStatusCodeActionResult statusResult
                ? statusResult.StatusCode ?? -1
                : -1;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        public static IEnumerable<object[]> ExceptionCases()
        {
            yield return new object[] { new InvalidDataException(), 400 };
            yield return new object[] { new KeyNotFoundException(), 404 };
            yield return new object[] { new DbUpdateException(), 304 };
            yield return new object[] { new DuplicateNameException(), 409 };
            yield return new object[] { new SecurityException(), 401 };
            yield return new object[] { new UnauthorizedAccessException(), 401 };
            yield return new object[] { new UserDisabledException("disabled"), 451 };
            yield return new object[] { new SecurityTokenExpiredException(), 410 };
            yield return new object[] { new Exception("unexpected"), 500 };
        }
    }
}
