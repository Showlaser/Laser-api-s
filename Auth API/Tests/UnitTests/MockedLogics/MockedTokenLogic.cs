using Auth_API.Logic;
using Auth_API.Tests.MockedDals;

namespace Auth_API.Tests.MockedLogics
{
    public class MockedTokenLogic
    {
        public readonly TokenLogic TokenLogic;

        public MockedTokenLogic()
        {
            MockedTokenDal tokenDal = new();
            TokenLogic = new TokenLogic(tokenDal.TokenDal);
        }
    }
}
