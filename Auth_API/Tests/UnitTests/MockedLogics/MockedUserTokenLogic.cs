using Auth_API.Logic;
using Auth_API.Tests.UnitTests.MockedDals;

namespace Auth_API.Tests.UnitTests.MockedLogics
{
    public class MockedUserTokenLogic
    {
        public readonly TokenLogic TokenLogic;

        public MockedUserTokenLogic()
        {
            MockedUserTokenDal userTokenDal = new();
            MockedSpotifyTokenDal spotifyTokenDal = new();
            TokenLogic = new TokenLogic(userTokenDal.UserTokenDal, spotifyTokenDal.SpotifyTokenDal);
        }
    }
}
