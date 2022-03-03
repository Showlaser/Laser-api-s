using Auth_API.Logic;
using Auth_API.Tests.MockedDals;

namespace Auth_API.Tests.MockedLogics
{
    public class MockedUserLogic
    {
        public readonly UserLogic UserLogic;

        public MockedUserLogic()
        {
            MockedUserDal userDal = new();
            UserLogic userLogic = new(userDal.UserDal);
            UserLogic = userLogic;
        }
    }
}
