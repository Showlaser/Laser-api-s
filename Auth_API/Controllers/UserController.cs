using Auth_API.Logic;
using Auth_API.Models.Dto.User;
using Auth_API.Models.FromFrontend.User;
using Auth_API.Models.Helper;
using Auth_API.Models.ToFrontend;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace Auth_API.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserLogic _userLogic;

        public UserController(UserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] User user)
        {
            async Task Action()
            {
                UserDto userDto = user.Adapt<UserDto>();
                await _userLogic.Add(userDto);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            await controllerErrorHandler.Execute(Action());
            return StatusCode(controllerErrorHandler.StatusCode);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken()
        {
            async Task<UserTokensViewmodel> Action()
            {
                IPAddress ip = Request.HttpContext.Connection.RemoteIpAddress ?? throw new NoNullAllowedException();
                UserTokensViewmodel tokens = ControllerHelper.GetUserTokens(this);
                return await _userLogic.RefreshToken(tokens, ip);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            UserTokensViewmodel tokens = await controllerErrorHandler.Execute(Action()) ?? throw new NoNullAllowedException();
            //TODO set cookie secure on true in production
            CookieOptions cookieOptions = new()
            {
                HttpOnly = true,
                Secure = false,
                Path = "/",
                Expires = DateTime.Now.AddDays(31)
            };

            Response.Cookies.Append("jwt", tokens.Jwt, cookieOptions);
            Response.Cookies.Append("refreshToken", tokens.RefreshToken, cookieOptions);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult?> Login([FromBody] User user)
        {
            async Task<ActionResult> Action()
            {
                IPAddress? ip = Request.HttpContext.Connection.RemoteIpAddress;
                UserDto userDto = user.Adapt<UserDto>();

                UserTokensViewmodel tokens = await _userLogic.Login(userDto, ip);
                //TODO set cookie secure on true in production
                CookieOptions cookieOptions = new()
                {
                    HttpOnly = true,
                    Secure = false,
                    Path = "/",
                    Expires = DateTime.Now.AddDays(7)
                };

                Response.Cookies.Append("jwt", tokens.Jwt, cookieOptions);
                Response.Cookies.Append("refreshToken", tokens.RefreshToken, cookieOptions);
                return Ok();
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return await controllerErrorHandler.Execute(Action());
        }

        [AuthorizedAction]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] User user)
        {
            async Task Action()
            {
                UserDto userData = ControllerHelper.GetUserModelFromJwtClaims(this);
                UserDto userDto = user.Adapt<UserDto>();
                userDto.Uuid = userData.Uuid;

                await _userLogic.Update(userDto, "123");
            }

            ControllerErrorHandler controllerErrorHandler = new();
            await controllerErrorHandler.Execute(Action());
            return StatusCode(controllerErrorHandler.StatusCode);
        }

        [AuthorizedAction]
        [HttpDelete]
        public async Task<ActionResult> Remove([FromBody] User user)
        {
            async Task Action()
            {
                UserDto userDto = user.Adapt<UserDto>();
                await _userLogic.Remove(userDto);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            await controllerErrorHandler.Execute(Action());
            return StatusCode(controllerErrorHandler.StatusCode);
        }
    }
}
