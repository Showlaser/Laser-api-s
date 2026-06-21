using Auth_API.CustomExceptions;
using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.Tokens;
using Auth_API.Models.ToFrontend;
using System.Data;
using System.Net;
using System.Net.Http.Headers;
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
        private readonly string _redirectUrl;
        private readonly string[] _scopes = { "user-read-currently-playing", "user-read-playback-state", "user-modify-playback-state", "user-read-private", "playlist-read-private", "user-library-read", "user-library-modify" };
        private readonly HttpClient _client;

        public SpotifyTokenLogic(ISpotifyTokenDal spotifyTokenDal, HttpClient client)
        {
            _spotifyTokenDal = spotifyTokenDal;
            _client = client;
            _clientId = Environment.GetEnvironmentVariable("SPOTIFYCLIENTID") ?? throw new NoNullAllowedException(
                "Environment variable SPOTIFYCLIENTID was empty. Set it using the SPOTIFYCLIENTID environment variable");
            _clientSecret = Environment.GetEnvironmentVariable("SPOTIFYCLIENTSECRET") ?? throw new NoNullAllowedException(
                "Environment variable SPOTIFYCLIENTSECRET was empty. Set it using the SPOTIFYCLIENTSECRET environment variable");
            _redirectUrl = Environment.GetEnvironmentVariable("SPOTIFYREDIRECTURL") ?? throw new NoNullAllowedException(
                "Environment variable SPOTIFYREDIRECTURL was empty. Set it using the SPOTIFYREDIRECTURL environment variable");
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

            return new string($"{AuthEndpoint}?response_type=code&client_id={_clientId}&redirect_uri={_redirectUrl}" +
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
                { "redirect_uri", _redirectUrl },
                { "code_verifier", dbTokens.CodeVerifier },
            };

            byte[] clientDataBytes = Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}");
            string clientDataBase64 = Convert.ToBase64String(clientDataBytes);

            FormUrlEncodedContent urlEncodedParameters = new(parameters);
            HttpRequestMessage req = new(HttpMethod.Post, "https://accounts.spotify.com/api/token")
            {
                Content = urlEncodedParameters,
                Headers = { Authorization = new AuthenticationHeaderValue("Basic", clientDataBase64) }
            };

            HttpResponseMessage response = await _client.SendAsync(req);
            if (!response.IsSuccessStatusCode)
            {
                throw new SecurityException("Could not exchange the authorization code with Spotify");
            }

            SpotifyTokensViewmodel? tokens = await response.Content.ReadFromJsonAsync<SpotifyTokensViewmodel>();
            if (tokens == null || string.IsNullOrEmpty(tokens.access_token) || string.IsNullOrEmpty(tokens.refresh_token))
            {
                throw new SecurityException("Spotify returned an invalid token response");
            }

            await UpdateSpotifyRefreshToken(userUuid, tokens.refresh_token);
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

            HttpResponseMessage response = await _client.SendAsync(req);
            if (!response.IsSuccessStatusCode)
            {
                string errorBody = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.BadRequest && errorBody.Contains("invalid_grant"))
                {
                    // De refresh token is verlopen of ingetrokken. De gebruiker moet
                    // opnieuw inloggen, dus geef een 401 terug zodat de frontend de
                    // opgeslagen token weggooit.
                    throw new SecurityException("The Spotify refresh token is no longer valid");
                }

                // Tijdelijke fout bij Spotify (rate limiting, storing). De refresh token
                // niet weggooien zodat een latere poging alsnog kan slagen.
                throw new SpotifyUnavailableException("Could not refresh the Spotify access token");
            }

            SpotifyTokensViewmodel? tokens = await response.Content.ReadFromJsonAsync<SpotifyTokensViewmodel>();
            if (tokens == null || string.IsNullOrEmpty(tokens.access_token) || string.IsNullOrEmpty(tokens.refresh_token))
            {
                throw new SecurityException("Invalid refresh token");
            }

            await UpdateSpotifyRefreshToken(userUuid, tokens.refresh_token);
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
