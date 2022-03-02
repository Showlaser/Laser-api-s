using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.User;
using Auth_API.Tests.TestModels;
using Moq;

namespace Auth_API.Tests.MockedDals
{
    public class MockedRefreshTokenDal
    {
        public readonly IRefreshTokenDal RefreshTokenDal;

        public MockedRefreshTokenDal()
        {
            TestRefreshTokenDto refreshToken = new();
            RefreshTokenDto token = refreshToken.RefreshToken;
            Mock<IRefreshTokenDal> mockedRefreshTokenDal = new();
            mockedRefreshTokenDal.Setup(rtd => rtd.Find(token.UserUuid)).ReturnsAsync(token);
            RefreshTokenDal = mockedRefreshTokenDal.Object;
        }
    }
}
