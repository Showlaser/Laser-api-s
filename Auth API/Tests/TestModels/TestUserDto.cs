using Auth_API.Models.Dto;

namespace Auth_API.Tests.TestModels
{
    public class TestUserDto
    {
        public readonly UserDto UserDto = new()
        {
            Uuid = Guid.Parse("4a4a4847-e081-40c8-a020-b5c2d4ccc00d"),
            UserName = "TestUser",
            Password = "55bcf43562cd823c5dee1998c668a2a4", // 123 plaintext
            Salt = "e1jEZS5djtf4bepz",
            SpotifyAccountData = new TestSpotifyAccountDataDto().SpotifyAccountDataDto
        };
    }
}
