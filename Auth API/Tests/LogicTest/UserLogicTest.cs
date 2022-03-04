using Auth_API.Logic;
using Auth_API.Models.Dto.User;
using Auth_API.Models.ToFrontend;
using Auth_API.Tests.MockedLogics;
using Auth_API.Tests.TestModels;
using NUnit.Framework;
using System.Net;
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
            UserTokensViewmodel tokens = await _userLogic.Login(new UserDto
            {
                Password = "123",
                Salt = _testUser.UserDto.Salt,
                UserName = _testUser.UserDto.UserName
            }, IPAddress.Parse("127.0.0.1"));
            Assert.IsTrue(tokens.Jwt.Length > 25);
            Assert.IsTrue(tokens.RefreshToken.Length > 25);
        }

        [Test]
        public void LoginWrongPasswordTest()
        {
            Assert.ThrowsAsync<SecurityException>(async () => await _userLogic.Login(_testUser.UserDto, IPAddress.Parse("127.0.0.1")));
        }

        [Test]
        public void RemoveUserTest()
        {
            Assert.DoesNotThrowAsync(async () => await _userLogic.Remove(_testUser.UserDto));
        }

        [Test]
        public async Task RefreshTokenTest()
        {
            UserTokensViewmodel tokens = await GetTokens();
            tokens.RefreshToken = @"r?p>L????p~???\u0017\b7Mr?n???E?+\\?k\u0015O??|?'?\u00108?,?4X?????\u0014?n??(\u00118????D?";
            UserTokensViewmodel token = await _userLogic.RefreshToken(new UserTokensViewmodel
            {
                Jwt = tokens.Jwt,
                RefreshToken = tokens.RefreshToken
            }, IPAddress.Parse("127.0.0.1"));

            Assert.IsTrue(token.RefreshToken.Length > 25);
            Assert.IsTrue(token.Jwt.Length > 25);
        }

        [Test]
        public async Task RefreshTokenWrongIpTest()
        {
            UserTokensViewmodel tokens = await GetTokens(IPAddress.Parse("127.0.0.2"));
            Assert.ThrowsAsync<SecurityException>(async () => await _userLogic.RefreshToken(new UserTokensViewmodel
            {
                Jwt = tokens.Jwt,
                RefreshToken = tokens.RefreshToken
            }, IPAddress.Parse("127.0.0.1")));
        }

        [Test]
        public async Task RefreshTokenWrongRefreshTokenTest()
        {
            UserTokensViewmodel tokens = await GetTokens();
            Assert.ThrowsAsync<SecurityException>(async () => await _userLogic.RefreshToken(new UserTokensViewmodel
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
