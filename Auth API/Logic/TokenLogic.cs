using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.User;
using IdentityModel.Client;
using System.Data;

namespace Auth_API.Logic
{
    public class TokenLogic
    {
        private readonly ITokenDal _tokenDal;

        public TokenLogic(ITokenDal tokenDal)
        {
            _tokenDal = tokenDal;
        }

        public async Task<string?> RefreshSpotifyAccessToken(Guid userUuid)
        {
            string clientId = Environment.GetEnvironmentVariable("SPOTIFYCLIENTID") ?? throw new NoNullAllowedException(
                "Environment variable SPOTIFYCLIENTID was empty. Set it using the SPOTIFYCLIENTID environment variable");
            string clientSecret = Environment.GetEnvironmentVariable("SPOTIFYCLIENTSECRET") ?? throw new NoNullAllowedException(
                "Environment variable SPOTIFYCLIENTSECRET was empty. Set it using the SPOTIFYCLIENTSECRET environment variable");

            UserTokensDto accountData = await _tokenDal.Find(userUuid) ?? throw new KeyNotFoundException();
            HttpClient client = new();
            TokenResponse response = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = "https://accounts.spotify.com/authorize",
                ClientId = clientId,
                ClientSecret = clientSecret,
                GrantType = "refresh_token",
                RefreshToken = accountData.RefreshToken
            });

            accountData.RefreshToken = response.RefreshToken;
            accountData.AccessToken = response.AccessToken;

            await _tokenDal.Remove(userUuid);
            await _tokenDal.Add(accountData);

            return response.AccessToken;
        }
    }
}
