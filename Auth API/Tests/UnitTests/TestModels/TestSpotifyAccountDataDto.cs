using Auth_API.Models.Dto.Tokens;

namespace Auth_API.Tests.UnitTests.TestModels
{
    public class TestSpotifyAccountDataDto
    {
        public readonly SpotifyTokensDto SpotifyTokensDto = new()
        {
            UserUuid = Guid.Parse("4a4a4847-e081-40c8-a020-b5c2d4ccc00d"),
            Uuid = Guid.Parse("c4c4cc81-8316-4ba5-ba69-af6217fb03db"),
            CodeVerifier = "jfejfw0e9fj2309f3jf089sjaf8jf2344",
            SpotifyRefreshToken = "wfhwn89f023hn9fweangfeaurgearheastrhearh"
        };
    }
}
