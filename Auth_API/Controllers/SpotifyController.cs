using Auth_API.Logic;
using Auth_API.Models.Dto.User;
using Auth_API.Models.FromFrontend.Spotify;
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

        [HttpPost("get-access-token")]
        public async Task<ActionResult<SpotifyTokensViewmodel?>> GetAccessToken([FromBody] SpotifyAccessTokenRequest request)
        {
            async Task<SpotifyTokensViewmodel?> Action()
            {
                UserDto user = ControllerHelper.GetUserModelFromJwtClaims(this);
                return await _spotifyTokenLogic.GetAccessToken(request.Code, user.Uuid);
            }

            return await _controllerResultHelper.Execute(Action());
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<SpotifyTokensViewmodel?>> RefreshAccessToken([FromBody] SpotifyRefreshTokenRequest request)
        {
            async Task<SpotifyTokensViewmodel?> Action()
            {
                UserDto user = ControllerHelper.GetUserModelFromJwtClaims(this);
                return await _spotifyTokenLogic.RefreshSpotifyAccessToken(request.RefreshToken, user.Uuid);
            }

            return await _controllerResultHelper.Execute(Action());
        }
    }
}
