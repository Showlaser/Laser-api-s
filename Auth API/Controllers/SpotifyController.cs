using Auth_API.Logic;
using Auth_API.Models.Dto.User;
using Auth_API.Models.Helper;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("grand-access")]
        public async Task<ActionResult<string?>> GrandAccessToSpotify()
        {
            async Task<string?> Action()
            {
                UserDto user = ControllerHelper.GetUserModelFromJwtClaims(this);
                return await _tokenLogic.GrandAccessToSpotify(user.Uuid);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return await controllerErrorHandler.Execute(Action());
        }

        [HttpGet("get-access-token")]
        public async Task<string?> GetAccessToken([FromQuery] string code)
        {
            async Task<string?> Action()
            {
                UserDto user = ControllerHelper.GetUserModelFromJwtClaims(this);
                return await _tokenLogic.GetAccessToken(code, user.Uuid);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return await controllerErrorHandler.Execute(Action());
        }

        [HttpGet("refresh")]
        public async Task<string?> RefreshAccessToken([FromQuery] string refreshToken)
        {
            async Task<string?> Action()
            {
                UserDto user = ControllerHelper.GetUserModelFromJwtClaims(this);
                return await _tokenLogic.RefreshSpotifyAccessToken(refreshToken, user.Uuid);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return await controllerErrorHandler.Execute(Action());
        }
    }
}
