using Auth_API.Logic;
using Auth_API.Tests.UnitTests.MockedDals;

namespace Auth_API.Tests.UnitTests.MockedLogics
{
    public class MockedUserTokenLogic
    {
        public readonly SpotifyTokenLogic SpotifyTokenLogic;

        public MockedUserTokenLogic()
        {
            MockedSpotifyTokenDal spotifyTokenDal = new();
            SpotifyTokenLogic = new SpotifyTokenLogic(spotifyTokenDal.SpotifyTokenDal);
        }
    }
}
