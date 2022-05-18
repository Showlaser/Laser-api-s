using Auth_API.Interfaces.Dal;
using Moq;

namespace Auth_API.Tests.UnitTests.MockedDals
{
    public class MockedDisabledUserDal
    {
        public readonly IDisabledUserDal DisabledUserDal;

        public MockedDisabledUserDal()
        {
            Mock<IDisabledUserDal>? disabledUserDal = new Mock<IDisabledUserDal>();
            DisabledUserDal = disabledUserDal.Object;
        }
    }
}
