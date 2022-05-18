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
            MockedUserActivationDal userActivationDal = new();
            MockedDisabledUserDal disabledUserDal = new();
            UserLogic userLogic = new(userDal.UserDal, userTokenDal.UserTokenDal, userActivationDal.UserActivationDal, disabledUserDal.DisabledUserDal);
            UserLogic = userLogic;
        }
    }
}
