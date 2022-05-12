using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.Tokens;
using Auth_API.Models.ToFrontend;
using System.Data;
using System.Security;
using System.Text;

namespace Auth_API.Logic
{
    public class SpotifyTokenLogic
    {
        private readonly ISpotifyTokenDal _spotifyTokenDal;
        private readonly string _clientSecret;
        private readonly string _clientId;
        private const string AuthEndpoint = "https://accounts.spotify.com/authorize";
        private const string RedirectUrl = "http://localhost:3000/laser-settings";
        private readonly string[] _scopes = { "user-read-currently-playing", "user-read-playback-state", "user-modify-playback-state", "user-read-private", "playlist-read-private" };
        private readonly HttpClient _client = new();

        public SpotifyTokenLogic(ISpotifyTokenDal spotifyTokenDal)
        {
            _spotifyTokenDal = spotifyTokenDal;
            _clientId = Environment.GetEnvironmentVariable("SPOTIFYCLIENTID") ?? throw new NoNullAllowedException(
                "Environment variable SPOTIFYCLIENTID was empty. Set it using the SPOTIFYCLIENTID environment variable");
            _clientSecret = Environment.GetEnvironmentVariable("SPOTIFYCLIENTSECRET") ?? throw new NoNullAllowedException(
                "Environment variable SPOTIFYCLIENTSECRET was empty. Set it using the SPOTIFYCLIENTSECRET environment variable");
        }

        public async Task<string> GrandAccessToSpotify(Guid userUuid)
        {
            string codeVerifier = SecurityLogic.GenerateNonce();
            string codeChallenge = SecurityLogic.GenerateCodeChallenge(codeVerifier);

            SpotifyTokensDto tokens = new()
            {
                Uuid = Guid.NewGuid(),
                UserUuid = userUuid,
                CodeVerifier = codeVerifier
            };

            await _spotifyTokenDal.Remove(userUuid);
            await _spotifyTokenDal.Add(tokens);

            return new string($"{AuthEndpoint}?response_type=code&client_id={_clientId}&redirect_uri={RedirectUrl}" +
                              $"&scope={string.Join("%20", _scopes)}&code_challenge={codeChallenge}&code_challenge_method=S256");
        }

        public async Task<SpotifyTokensViewmodel?> GetAccessToken(string code, Guid userUuid)
        {
            SpotifyTokensDto dbTokens = await _spotifyTokenDal.Find(userUuid) ?? throw new SecurityException();
            Dictionary<string, string> parameters = new()
            {
                { "client_id", _clientId },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", RedirectUrl },
                { "code_verifier", dbTokens.CodeVerifier },
            };

            byte[] clientDataBytes = Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}");
            string clientDataBase64 = Convert.ToBase64String(clientDataBytes);

            _client.DefaultRequestHeaders.Add("Authorization", $"Basic {clientDataBase64}");
            FormUrlEncodedContent urlEncodedParameters = new(parameters);
            HttpRequestMessage req = new(HttpMethod.Post, "https://accounts.spotify.com/api/token") { Content = urlEncodedParameters };

            req.Content = urlEncodedParameters;
            HttpResponseMessage response = await _client.SendAsync(req);
            SpotifyTokensViewmodel? tokens = await response.Content.ReadFromJsonAsync<SpotifyTokensViewmodel>();

            await UpdateSpotifyRefreshToken(userUuid, tokens?.refresh_token);
            return tokens;
        }

        public async Task<SpotifyTokensViewmodel?> RefreshSpotifyAccessToken(string refreshToken, Guid userUuid)
        {
            if (refreshToken.Length < 15)
            {
                throw new InvalidDataException();
            }

            Dictionary<string, string> parameters = new()
            {
                { "client_id", _clientId },
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
            };

            FormUrlEncodedContent urlEncodedParameters = new(parameters);
            HttpRequestMessage req = new(HttpMethod.Post, "https://accounts.spotify.com/api/token") { Content = urlEncodedParameters };

            req.Content = urlEncodedParameters;
            HttpResponseMessage response = await _client.SendAsync(req);
            SpotifyTokensViewmodel? tokens = await response.Content.ReadFromJsonAsync<SpotifyTokensViewmodel>();

            await UpdateSpotifyRefreshToken(userUuid, tokens?.refresh_token);
            return tokens;
        }

        private async Task UpdateSpotifyRefreshToken(Guid userUuid, string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new NoNullAllowedException(nameof(refreshToken));
            }

            SpotifyTokensDto dbTokens = await _spotifyTokenDal.Find(userUuid) ?? throw new SecurityException();
            dbTokens.SpotifyRefreshToken = refreshToken;

            await _spotifyTokenDal.Remove(userUuid);
            await _spotifyTokenDal.Add(dbTokens);
        }
    }
}
