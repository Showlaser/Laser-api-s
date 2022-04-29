using Auth_API.Logic;
using Auth_API.Tests.UnitTests.MockedDals;

namespace Auth_API.Tests.UnitTests.MockedLogics
{
    public class MockedUserLogic
    {
        public readonly UserLogic UserLogic;

        public MockedUserLogic()
        {
            MockedUserDal userDal = new();
            MockedUserTokenDal userTokenDal = new();
            UserLogic userLogic = new(userDal.UserDal, userTokenDal.UserTokenDal, null, null);
            UserLogic = userLogic;
        }
    }
}
