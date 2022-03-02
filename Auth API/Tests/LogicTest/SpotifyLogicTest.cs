using Auth_API.Logic;
using Auth_API.Models.Dto.Spotify;
using Auth_API.Tests.MockedLogics;
using Auth_API.Tests.TestModels;
using NUnit.Framework;
using System.Security;

namespace Auth_API.Tests.LogicTest
{
    [TestFixture]
    public class SpotifyLogicTest
    {
        private readonly SpotifyLogic _spotifyLogic;
        private readonly SpotifyAccountDataDto _accountData;

        public SpotifyLogicTest()
        {
            _spotifyLogic = new MockedSpotifyLogic().SpotifyLogic;
            TestSpotifyAccountDataDto testSpotifyAccountData = new();
            _accountData = testSpotifyAccountData.SpotifyAccountDataDto;
        }

        [Test]
        public async Task RefreshAccessTokenTest()
        {
            string newAccessToken = await _spotifyLogic.RefreshAccessToken(_accountData.UserUuid);
            Assert.IsNotNull(newAccessToken);
        }

        [Test]
        public async Task RefreshAccessTokenKeyNotFoundExceptionTest()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _spotifyLogic.RefreshAccessToken(_accountData.UserUuid));
        }

        [Test]
        public async Task RefreshAccessTokenSecurityExceptionTest()
        {
            Assert.ThrowsAsync<SecurityException>(async () => await _spotifyLogic.RefreshAccessToken(Guid.Parse("e2e177fb-e560-42f3-8d4d-07a5c14acf4b")));
        }
    }
}
