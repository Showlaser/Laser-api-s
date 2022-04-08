using Auth_API.Logic;
using Auth_API.Models.Dto.User;
using Auth_API.Models.Helper;
using Auth_API.Models.ToFrontend;
using Microsoft.AspNetCore.Mvc;

namespace Auth_API.Controllers
{
    [AuthorizedAction]
    [Route("spotify")]
    [ApiController]
    public class SpotifyController : ControllerBase
    {
        private readonly SpotifyTokenLogic _spotifyTokenLogic;

        public SpotifyController(SpotifyTokenLogic spotifyTokenLogic)
        {
            _spotifyTokenLogic = spotifyTokenLogic;
        }

        [HttpGet("grand-access")]
        public async Task<ActionResult<string?>> GrandAccessToSpotify()
        {
            async Task<string?> Action()
            {
                UserDto user = ControllerHelper.GetUserModelFromJwtClaims(this);
                return await _spotifyTokenLogic.GrandAccessToSpotify(user.Uuid);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return await controllerErrorHandler.Execute(Action());
        }

        [HttpGet("get-access-token")]
        public async Task<ActionResult<SpotifyTokensViewmodel?>> GetAccessToken([FromQuery] string code)
        {
            async Task<SpotifyTokensViewmodel?> Action()
            {
                UserDto user = ControllerHelper.GetUserModelFromJwtClaims(this);
                return await _spotifyTokenLogic.GetAccessToken(code, user.Uuid);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return await controllerErrorHandler.Execute(Action());
        }

        [HttpGet("refresh")]
        public async Task<ActionResult<SpotifyTokensViewmodel?>> RefreshAccessToken([FromQuery] string refreshToken)
        {
            async Task<SpotifyTokensViewmodel?> Action()
            {
                UserDto user = ControllerHelper.GetUserModelFromJwtClaims(this);
                return await _spotifyTokenLogic.RefreshSpotifyAccessToken(refreshToken, user.Uuid);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return await controllerErrorHandler.Execute(Action());
        }
    }
}
