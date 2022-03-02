using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.Spotify;
using Moq;

namespace Auth_API.Tests.MockedDals
{
    public class MockedSpotifyDal
    {
        public readonly ISpotifyDal SpotifyDal;

        public MockedSpotifyDal()
        {
            SpotifyAccountDataDto accountDataDto = new();
            Mock<ISpotifyDal> mockedSpotifyDal = new();
            mockedSpotifyDal.Setup(msd => msd.Find(accountDataDto.UserUuid)).ReturnsAsync(accountDataDto);
            SpotifyDal = mockedSpotifyDal.Object;
        }
    }
}
