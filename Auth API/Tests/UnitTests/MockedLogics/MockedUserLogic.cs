using Auth_API.Logic;
using Auth_API.Tests.MockedDals;

namespace Auth_API.Tests.UnitTests.MockedLogics
{
    public class MockedUserLogic
    {
        public readonly UserLogic UserLogic;

        public MockedUserLogic()
        {
            MockedUserDal userDal = new();
            MockedTokenDal tokenDal = new();
            UserLogic userLogic = new(userDal.UserDal, tokenDal.TokenDal);
            UserLogic = userLogic;
        }
    }
}
