using Auth_API.Logic;
using Auth_API.Tests.MockedDals;

namespace Auth_API.Tests.MockedLogics
{
    public class MockedSpotifyLogic
    {
        public readonly SpotifyLogic SpotifyLogic;

        public MockedSpotifyLogic()
        {
            MockedSpotifyDal spotifyDal = new();
            SpotifyLogic = new SpotifyLogic(spotifyDal.SpotifyDal);
        }
    }
}
