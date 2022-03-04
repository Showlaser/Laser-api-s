using Auth_API.Logic;
using Auth_API.Models.Dto.User;
using Auth_API.Tests.MockedLogics;
using Auth_API.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Auth_API.Tests.LogicTest
{
    [TestClass]
    public class SpotifyLogicTest
    {
        private readonly TokenLogic _tokenLogic;
        private readonly UserTokensDto _accountData;

        public SpotifyLogicTest()
        {
            _tokenLogic = new MockedTokenLogic().TokenLogic;
            TestSpotifyAccountDataDto testSpotifyAccountData = new();
            _accountData = testSpotifyAccountData.UserTokensDto;
            Environment.SetEnvironmentVariable("SPOTIFYCLIENTID", "123");
            Environment.SetEnvironmentVariable("SPOTIFYCLIENTSECRET", "123");
        }

        [TestMethod]
        public void RefreshAccessTokenKeyNotFoundExceptionTest()
        {
            Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await _tokenLogic.RefreshSpotifyAccessToken(Guid.Empty));
        }
    }
}
