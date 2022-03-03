using Auth_API.Logic;
using Auth_API.Models.Dto.User;
using Auth_API.Tests.MockedLogics;
using Auth_API.Tests.TestModels;
using NUnit.Framework;
using System.Security;

namespace Auth_API.Tests.LogicTest
{
    [TestFixture]
    public class UserLogicTest
    {
        private readonly UserLogic _userLogic;
        private readonly TestUserDto _testUser = new();

        public UserLogicTest()
        {
            _userLogic = new MockedUserLogic().UserLogic;
            Environment.SetEnvironmentVariable("ARGON2SECRET", "hhjwe093892349jsdfwe");
            Environment.SetEnvironmentVariable("JWTSECRET", "fwhefwiufhawgh98g43hg98ahdfjig");
        }

        [Test]
        public void AddUserTest()
        {
            Assert.DoesNotThrowAsync(async () => await _userLogic.Add(_testUser.UserDto));
        }

        [Test]
        public async Task LoginTest()
        {
            string jwt = await _userLogic.Login(new UserDto
            {
                Password = "123",
                Salt = _testUser.UserDto.Salt,
                UserName = _testUser.UserDto.UserName
            });
            Assert.IsTrue(jwt.Length > 25);
        }

        [Test]
        public void LoginWrongPasswordTest()
        {
            Assert.ThrowsAsync<SecurityException>(async () => await _userLogic.Login(_testUser.UserDto));
        }

        [Test]
        public void RemoveUserTest()
        {
            Assert.DoesNotThrowAsync(async () => await _userLogic.Remove(_testUser.UserDto));
        }
    }
}
