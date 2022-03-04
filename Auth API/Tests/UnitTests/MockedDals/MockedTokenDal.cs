using Auth_API.Interfaces.Dal;
using Auth_API.Tests.TestModels;
using Moq;

namespace Auth_API.Tests.MockedDals
{
    public class MockedTokenDal
    {
        public readonly ITokenDal TokenDal;

        public MockedTokenDal()
        {
            TestRefreshTokenDto accountDataDto = new();
            Mock<ITokenDal> mockedTokenDal = new();
            mockedTokenDal.Setup(msd => msd.Find(accountDataDto.RefreshToken.UserUuid)).ReturnsAsync(accountDataDto.RefreshToken);
            TokenDal = mockedTokenDal.Object;
        }
    }
}
