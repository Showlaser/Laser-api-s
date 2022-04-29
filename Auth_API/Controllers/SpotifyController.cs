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
        private readonly ControllerResultHelper _controllerResultHelper;

        public SpotifyController(SpotifyTokenLogic spotifyTokenLogic, ControllerResultHelper controllerResultHelper)
        {
            _spotifyTokenLogic = spotifyTokenLogic;
            _controllerResultHelper = controllerResultHelper;
        }

        [HttpGet("grand-access")]
        public async Task<ActionResult<string?>> GrandAccessToSpotify()
        {
            async Task<string?> Action()
            {
                UserDto user = ControllerHelper.GetUserModelFromJwtClaims(this);
                return await _spotifyTokenLogic.GrandAccessToSpotify(user.Uuid);
            }

            return await _controllerResultHelper.Execute(Action());
        }

        [HttpGet("get-access-token")]
        public async Task<ActionResult<SpotifyTokensViewmodel?>> GetAccessToken([FromQuery] string code)
        {
            async Task<SpotifyTokensViewmodel?> Action()
            {
                UserDto user = ControllerHelper.GetUserModelFromJwtClaims(this);
                return await _spotifyTokenLogic.GetAccessToken(code, user.Uuid);
            }

            return await _controllerResultHelper.Execute(Action());
        }

        [HttpGet("refresh")]
        public async Task<ActionResult<SpotifyTokensViewmodel?>> RefreshAccessToken([FromQuery] string refreshToken)
        {
            async Task<SpotifyTokensViewmodel?> Action()
            {
                UserDto user = ControllerHelper.GetUserModelFromJwtClaims(this);
                return await _spotifyTokenLogic.RefreshSpotifyAccessToken(refreshToken, user.Uuid);
            }

            return await _controllerResultHelper.Execute(Action());
        }
    }
}
