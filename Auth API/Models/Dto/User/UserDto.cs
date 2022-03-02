using Auth_API.Models.Dto.Spotify;

namespace Auth_API.Models.Dto.User
{
    public class UserDto
    {
        public Guid Uuid { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public SpotifyAccountDataDto SpotifyAccountData { get; set; }
    }
}
