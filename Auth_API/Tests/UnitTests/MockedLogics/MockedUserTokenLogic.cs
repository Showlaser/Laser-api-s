using Auth_API.Logic;
using Auth_API.Tests.UnitTests.MockedDals;

namespace Auth_API.Tests.UnitTests.MockedLogics
{
    public class MockedUserTokenLogic
    {
        public readonly SpotifyTokenLogic SpotifyTokenLogic;

        public MockedUserTokenLogic(HttpMessageHandler? httpMessageHandler = null)
        {
            MockedSpotifyTokenDal spotifyTokenDal = new();
            HttpClient client = httpMessageHandler == null ? new HttpClient() : new HttpClient(httpMessageHandler);
            SpotifyTokenLogic = new SpotifyTokenLogic(spotifyTokenDal.SpotifyTokenDal, client);
        }
    }
}
