using Auth_API.Logic;
using Auth_API.Tests.UnitTests.MockedLogics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Auth_API.Tests.UnitTests.LogicTest
{
    [TestClass]
    public class SpotifyLogicTest
    {
        private readonly TokenLogic _tokenLogic;

        public SpotifyLogicTest()
        {
            TestHelper.SetEnvironmentVariables();
            _tokenLogic = new MockedUserTokenLogic().TokenLogic;
        }
    }
}
