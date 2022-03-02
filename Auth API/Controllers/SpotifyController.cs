using Auth_API.Logic;
using Auth_API.Models.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Security;
using System.Security.Claims;

namespace Auth_API.Controllers
{
    [Route("spotify")]
    [ApiController]
    public class SpotifyController : ControllerBase
    {
        private readonly SpotifyLogic _spotifyLogic;

        public SpotifyController(SpotifyLogic spotifyLogic)
        {
            _spotifyLogic = spotifyLogic;
        }

        [HttpPost]
        public async Task<string> RefreshAccessToken()
        {
            async Task<string> Action()
            {
                string bearer = Request.Headers[HeaderNames.Authorization]
                    .ToString()
                    .Replace("Bearer ", "");
                if (HttpContext.User.Identity is not ClaimsIdentity identity)
                {
                    throw new SecurityException("Jwt not valid");
                }

                string claimValue = identity.FindFirst("userUuid")?.Value ?? throw new SecurityException("Invalid claim");
                Guid userUuid = Guid.Parse(claimValue);
                return await _spotifyLogic.RefreshAccessToken(userUuid);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return await controllerErrorHandler.Execute(Action());
        }
    }
}
