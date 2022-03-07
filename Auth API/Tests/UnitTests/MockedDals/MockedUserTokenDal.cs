using Auth_API.Interfaces.Dal;
using Auth_API.Tests.UnitTests.TestModels;
using Moq;

namespace Auth_API.Tests.UnitTests.MockedDals
{
    public class MockedUserTokenDal
    {
        public readonly IUserTokenDal UserTokenDal;

        public MockedUserTokenDal()
        {
            TestRefreshTokenDto accountDataDto = new();
            Mock<IUserTokenDal> mockedTokenDal = new();
            mockedTokenDal.Setup(msd => msd.Find(accountDataDto.RefreshToken.UserUuid)).ReturnsAsync(accountDataDto.RefreshToken);
            UserTokenDal = mockedTokenDal.Object;
        }
    }
}
