using Auth_API.Models.Dto.User;
using Auth_API.Tests.TestModels;

namespace Auth_API.Tests.UnitTests.TestModels
{
    public class TestUserDto
    {
        public readonly UserDto UserDto = new()
        {
            Uuid = Guid.Parse("4a4a4847-e081-40c8-a020-b5c2d4ccc00d"),
            UserName = "TestUser",
            Password = "$argon2i$v=19$m=32768,t=10,p=5$p54p6MBmr+tV05Nr9Uly8ElJ5Sr7+Ga2EhTZZcO3O1UyLPCFQzLIkGyFPKKenTAyAtg1KDB9gRqxC2uWOstUYg$HclRmE06yFmsMbICIxSnyplzROiOil0ZsfoC1fU60T8", // 123 plaintext
            Salt = new byte[64],
            SpotifyAccountData = new TestSpotifyAccountDataDto().UserTokensDto
        };
    }
}
