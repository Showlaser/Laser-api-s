using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.User;
using Moq;

namespace Auth_API.Tests.MockedDals
{
    public class MockedTokenDal
    {
        public readonly ITokenDal TokenDal;

        public MockedTokenDal()
        {
            UserTokensDto accountDataDto = new();
            Mock<ITokenDal> mockedTokenDal = new();
            mockedTokenDal.Setup(msd => msd.Find(accountDataDto.UserUuid)).ReturnsAsync(accountDataDto);
            TokenDal = mockedTokenDal.Object;
        }
    }
}
