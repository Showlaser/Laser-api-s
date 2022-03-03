using Auth_API.Logic;
using Auth_API.Models.Dto.User;
using Auth_API.Tests.MockedLogics;
using Auth_API.Tests.TestModels;
using NUnit.Framework;
using System.Security;

namespace Auth_API.Tests.LogicTest
{
    [TestFixture]
    public class SpotifyLogicTest
    {
        private readonly TokenLogic _tokenLogic;
        private readonly UserTokensDto _accountData;

        public SpotifyLogicTest()
        {
            _tokenLogic = new MockedTokenLogic().TokenLogic;
            TestSpotifyAccountDataDto testSpotifyAccountData = new();
            _accountData = testSpotifyAccountData.UserTokensDto;
        }

        [Test]
        public async Task RefreshAccessTokenTest()
        {
            string? newAccessToken = await _tokenLogic.RefreshSpotifyAccessToken(_accountData.UserUuid);
            Assert.IsNotNull(newAccessToken);
        }

        [Test]
        public void RefreshAccessTokenKeyNotFoundExceptionTest()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _tokenLogic.RefreshSpotifyAccessToken(_accountData.UserUuid));
        }

        [Test]
        public void RefreshAccessTokenSecurityExceptionTest()
        {
            Assert.ThrowsAsync<SecurityException>(async () => await _tokenLogic.RefreshSpotifyAccessToken(Guid.Parse("e2e177fb-e560-42f3-8d4d-07a5c14acf4b")));
        }
    }
}
