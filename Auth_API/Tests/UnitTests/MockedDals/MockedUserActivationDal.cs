using Auth_API.Interfaces.Dal;
using Moq;

namespace Auth_API.Tests.UnitTests.MockedDals
{
    public class MockedUserActivationDal
    {
        public readonly IUserActivationDal UserActivationDal;

        public MockedUserActivationDal()
        {
            Mock<IUserActivationDal>? userActivationDal = new Mock<IUserActivationDal>();
            UserActivationDal = userActivationDal.Object;
        }
    }
}
