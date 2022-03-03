using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.User;
using Auth_API.Tests.TestModels;
using Moq;

namespace Auth_API.Tests.MockedDals
{
    public class MockedRefreshTokenDal
    {
        public readonly ITokenDal TokenDal;

        public MockedRefreshTokenDal()
        {
            TestRefreshTokenDto refreshToken = new();
            UserTokensDto token = refreshToken.RefreshToken;
            Mock<ITokenDal> mockedRefreshTokenDal = new();
            mockedRefreshTokenDal.Setup(rtd => rtd.Find(token.UserUuid)).ReturnsAsync(token);
            TokenDal = mockedRefreshTokenDal.Object;
        }
    }
}
