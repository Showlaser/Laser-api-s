using Auth_API.Interfaces.Dal;
using Auth_API.Tests.UnitTests.TestModels;
using Moq;

namespace Auth_API.Tests.UnitTests.MockedDals
{
    public class MockedSpotifyTokenDal
    {
        public readonly ISpotifyTokenDal SpotifyTokenDal;

        public MockedSpotifyTokenDal()
        {
            TestSpotifyAccountDataDto accountDataDto = new();
            Mock<ISpotifyTokenDal> mockedTokenDal = new();
            mockedTokenDal.Setup(msd => msd.Find(accountDataDto.SpotifyTokensDto.UserUuid)).ReturnsAsync(accountDataDto.SpotifyTokensDto);
            SpotifyTokenDal = mockedTokenDal.Object;
        }
    }
}
