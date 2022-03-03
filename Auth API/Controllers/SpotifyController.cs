using Auth_API.Logic;
using Auth_API.Models.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Security;
using System.Security.Claims;

namespace Auth_API.Controllers
{
    [AuthorizedAction]
    [Route("spotify")]
    [ApiController]
    public class SpotifyController : ControllerBase
    {
        private readonly TokenLogic _tokenLogic;

        public SpotifyController(TokenLogic tokenLogic)
        {
            _tokenLogic = tokenLogic;
        }

        [HttpPost]
        public async Task<string?> RefreshAccessToken()
        {
            async Task<string?> Action()
            {
                string jwt = Request.Headers[HeaderNames.Authorization]
                    .ToString()
                    .Replace("Bearer ", "");
                List<Claim> claims = JwtLogic.GetJwtClaims(jwt);

                string claimValue = claims.Find(c => c.Type == "uuid")?.Value ?? throw new SecurityException("Invalid claim");
                Guid userUuid = Guid.Parse(claimValue);
                return await _tokenLogic.RefreshSpotifyAccessToken(userUuid);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return await controllerErrorHandler.Execute(Action());
        }
    }
}
