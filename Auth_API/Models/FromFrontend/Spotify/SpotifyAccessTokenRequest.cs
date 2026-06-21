using System.ComponentModel.DataAnnotations;

namespace Auth_API.Models.FromFrontend.Spotify
{
    public class SpotifyAccessTokenRequest
    {
        [Required]
        public string Code { get; set; } = string.Empty;
    }
}
