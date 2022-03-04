namespace Auth_API.Models.ToFrontend
{
    public class UserTokensViewmodel
    {
        public string Jwt { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
