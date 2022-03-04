using Auth_API.Models.Dto.User;
using System.Net;

namespace Auth_API.Tests.TestModels
{
    public class TestRefreshTokenDto
    {
        public readonly UserTokensDto RefreshToken = new()
        {
            Uuid = Guid.Parse("d3486567-2bf9-40e4-94af-7455de305569"),
            UserUuid = new TestUserDto().UserDto.Uuid,
            RefreshToken = "YgFEPbi9VHayH85TwaQe4+ld0PIjgfYQAM3P6nNXkE1KyHXyQs76IyekCzB8WD5pAwV6iixxMc8IY0GXDTM5Ow==",
            SpotifyRefreshToken = "",
            ClientIp = IPAddress.Parse("127.0.0.1"),
            RefreshTokenExpireDate = DateTime.Now.AddDays(7)
        };
    }
}
