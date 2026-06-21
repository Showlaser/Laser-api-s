using Auth_API.CustomExceptions;
using Auth_API.Logic;
using Auth_API.Tests.UnitTests.MockedLogics;
using Auth_API.Tests.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Net;
using System.Security;

namespace Auth_API.Tests.UnitTests.LogicTest
{
    [TestClass]
    public class SpotifyLogicAdditionalTest
    {
        private readonly SpotifyTokenLogic _spotifyTokenLogic;
        private readonly TestSpotifyAccountDataDto _testTokens = new();

        public SpotifyLogicAdditionalTest()
        {
            TestHelper.SetupTestEnvironment();
            _spotifyTokenLogic = new MockedUserTokenLogic().SpotifyTokenLogic;
        }

        [TestMethod]
        public async Task GrandAccessReturnsAuthorizeUrlWithPkceTest()
        {
            string url = await _spotifyTokenLogic.GrandAccessToSpotify(_testTokens.SpotifyTokensDto.UserUuid);

            StringAssert.Contains(url, "https://accounts.spotify.com/authorize");
            StringAssert.Contains(url, "response_type=code");
            StringAssert.Contains(url, "client_id=");
            StringAssert.Contains(url, "code_challenge=");
            StringAssert.Contains(url, "code_challenge_method=S256");
        }

        [TestMethod]
        public async Task RefreshWithTooShortTokenThrowsTest()
        {
            await Assert.ThrowsAsync<InvalidDataException>(async () =>
                await _spotifyTokenLogic.RefreshSpotifyAccessToken("short", _testTokens.SpotifyTokensDto.UserUuid));
        }

        [TestMethod]
        public async Task RefreshWithExpiredTokenThrowsSecurityExceptionTest()
        {
            FakeHttpMessageHandler handler = new(HttpStatusCode.BadRequest, "{\"error\":\"invalid_grant\"}");
            SpotifyTokenLogic spotifyTokenLogic = new MockedUserTokenLogic(handler).SpotifyTokenLogic;

            await Assert.ThrowsAsync<SecurityException>(async () =>
                await spotifyTokenLogic.RefreshSpotifyAccessToken(_testTokens.SpotifyTokensDto.SpotifyRefreshToken!,
                    _testTokens.SpotifyTokensDto.UserUuid));
        }

        [TestMethod]
        public async Task RefreshWithTransientSpotifyErrorThrowsUnavailableExceptionTest()
        {
            FakeHttpMessageHandler handler = new(HttpStatusCode.ServiceUnavailable);
            SpotifyTokenLogic spotifyTokenLogic = new MockedUserTokenLogic(handler).SpotifyTokenLogic;

            await Assert.ThrowsAsync<SpotifyUnavailableException>(async () =>
                await spotifyTokenLogic.RefreshSpotifyAccessToken(_testTokens.SpotifyTokensDto.SpotifyRefreshToken!,
                    _testTokens.SpotifyTokensDto.UserUuid));
        }
    }
}
