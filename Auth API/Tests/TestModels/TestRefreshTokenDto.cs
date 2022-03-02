using Auth_API.Models.Dto.User;

namespace Auth_API.Tests.TestModels
{
    public class TestRefreshTokenDto
    {
        public readonly RefreshTokenDto RefreshToken = new()
        {
            Uuid = Guid.Parse("d3486567-2bf9-40e4-94af-7455de305569"),
            UserUuid = new TestUserDto().UserDto.Uuid,
            RefreshToken = "",
            ExpirationDate = DateTime.Now.AddDays(7)
        };
    }
}
