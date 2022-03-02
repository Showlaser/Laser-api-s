namespace Auth_API.Models.Dto.Spotify
{
    public class SpotifyAccountDataDto
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
