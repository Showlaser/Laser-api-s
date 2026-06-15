using System.ComponentModel.DataAnnotations;

namespace Auth_API.Models.FromFrontend.Spotify
{
    public class SpotifyRefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
