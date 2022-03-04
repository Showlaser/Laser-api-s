using Auth_API.Logic;
using Auth_API.Models.Dto.User;
using Auth_API.Models.ToFrontend;
using Auth_API.Tests.TestModels;
using Auth_API.Tests.UnitTests.MockedLogics;
using Auth_API.Tests.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Security;

namespace Auth_API.Tests.UnitTests.LogicTest
{
    [TestClass]
    public class UserLogicTest
    {
        private readonly UserLogic _userLogic;
        private readonly TestUserDto _testUser = new();

        public UserLogicTest()
        {
            _userLogic = new MockedUserLogic().UserLogic;
            TestHelper.SetEnvironmentVariables();
        }

        [TestMethod]
        public async Task AddUserTest()
        {
            await _userLogic.Add(_testUser.UserDto);
        }

        [TestMethod]
        public async Task LoginTest()
        {
            UserTokensViewmodel tokens = await _userLogic.Login(new UserDto
            {
                Password = "123",
                Salt = _testUser.UserDto.Salt,
                UserName = _testUser.UserDto.UserName
            }, IPAddress.Parse("127.0.0.1"));
            Assert.IsTrue(tokens.Jwt.Length > 25);
            Assert.IsTrue(tokens.RefreshToken.Length > 25);
        }

        [TestMethod]
        public void LoginWrongPasswordTest()
        {
            Assert.ThrowsExceptionAsync<SecurityException>(async () => await _userLogic.Login(_testUser.UserDto, IPAddress.Parse("127.0.0.1")));
        }

        [TestMethod]
        public async Task RemoveUserTest()
        {
            await _userLogic.Remove(_testUser.UserDto);
        }

        [TestMethod]
        public async Task RefreshTokenTest()
        {
            UserTokensViewmodel tokens = await GetTokens();
            tokens.RefreshToken = new TestRefreshTokenDto().RefreshToken.RefreshToken;
            UserTokensViewmodel token = await _userLogic.RefreshToken(new UserTokensViewmodel
            {
                Jwt = tokens.Jwt,
                RefreshToken = tokens.RefreshToken
            }, IPAddress.Parse("127.0.0.1"));

            Assert.IsTrue(token.RefreshToken.Length > 25);
            Assert.IsTrue(token.Jwt.Length > 25);
        }

        [TestMethod]
        public async Task RefreshTokenWrongIpTest()
        {
            UserTokensViewmodel tokens = await GetTokens(IPAddress.Parse("127.0.0.2"));
            await Assert.ThrowsExceptionAsync<SecurityException>(async () => await _userLogic.RefreshToken(new UserTokensViewmodel
            {
                Jwt = tokens.Jwt,
                RefreshToken = tokens.RefreshToken
            }, IPAddress.Parse("127.0.0.1")));
        }

        [TestMethod]
        public async Task RefreshTokenWrongRefreshTokenTest()
        {
            UserTokensViewmodel tokens = await GetTokens();
            await Assert.ThrowsExceptionAsync<SecurityException>(async () => await _userLogic.RefreshToken(new UserTokensViewmodel
            {
                Jwt = tokens.Jwt,
                RefreshToken = tokens.RefreshToken
            }, IPAddress.Parse("127.0.0.1")));
        }

        private async Task<UserTokensViewmodel> GetTokens(IPAddress? ip = null)
        {
            ip ??= IPAddress.Parse("127.0.0.1");
            return await _userLogic.Login(new UserDto
            {
                Password = "123",
                Salt = _testUser.UserDto.Salt,
                UserName = _testUser.UserDto.UserName
            }, ip);
        }
    }
}
