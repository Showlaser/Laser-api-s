using System.Net;

namespace Auth_API.Models.Dto.User
{
    public class UserTokensDto
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpireDate { get; set; }
        public IPAddress? ClientIp { get; set; }
        public string SpotifyRefreshToken { get; set; } = string.Empty;
    }
}
