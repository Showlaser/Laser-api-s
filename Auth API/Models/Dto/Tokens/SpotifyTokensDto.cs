namespace Auth_API.Models.Dto.Tokens
{
    public class SpotifyTokensDto
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public string? CodeVerifier { get; set; }
        public string? SpotifyRefreshToken { get; set; }
    }
}
