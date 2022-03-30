using Auth_API.Logic;
using Auth_API.Tests.UnitTests.MockedLogics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Auth_API.Tests.UnitTests.LogicTest
{
    [TestClass]
    public class SpotifyLogicTest
    {
        private readonly SpotifyTokenLogic _spotifyTokenLogic;

        public SpotifyLogicTest()
        {
            TestHelper.SetEnvironmentVariables();
            _spotifyTokenLogic = new MockedUserTokenLogic().SpotifyTokenLogic;
        }
    }
}
