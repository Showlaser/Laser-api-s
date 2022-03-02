using Auth_API.Logic;
using Auth_API.Models.Dto;
using Auth_API.Tests.MockedLogics;
using Auth_API.Tests.TestModels;
using NUnit.Framework;

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
        public void RefreshAccessTokenTest()
        {
            _spotifyLogic.RefreshAccessToken(_accountData.AccessToken);
        }
    }
}
