using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.Spotify;
using IdentityModel.Client;

namespace Auth_API.Logic
{
    public class SpotifyLogic
    {
        private readonly ISpotifyDal _spotifyDal;

        public SpotifyLogic(ISpotifyDal spotifyDal)
        {
            _spotifyDal = spotifyDal;
        }

        public async Task<string> RefreshAccessToken(Guid userUuid)
        {
            SpotifyAccountDataDto accountData = await _spotifyDal.Find(userUuid) ?? throw new KeyNotFoundException();
            HttpClient client = new();
            TokenResponse response = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = "https://accounts.spotify.com/authorize",
                ClientId = "b63c814d37454bb4bb47667cc2a854e0",
                ClientSecret = "3461856b314c49408ee37ecd6d3c9413",
                GrantType = "refresh_token",
                RefreshToken = accountData.RefreshToken
            });

            accountData.RefreshToken = response.RefreshToken;
            accountData.AccessToken = response.AccessToken;

            await _spotifyDal.Remove(userUuid);
            await _spotifyDal.Add(accountData);

            return response.AccessToken;
        }
    }
}
