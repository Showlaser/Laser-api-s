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
        private readonly ControllerResultHelper _controllerResultHelper;

        public UserController(UserLogic userLogic, ControllerResultHelper controllerResultHelper)
        {
            _userLogic = userLogic;
            _controllerResultHelper = controllerResultHelper;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] User user)
        {
            async Task Action()
            {
                UserDto userDto = user.Adapt<UserDto>();
                await _userLogic.Add(userDto);
            }

            ControllerResultHelper controllerResultHelper = new();
            return await controllerResultHelper.Execute(Action());
        }

        [HttpGet]
        public async Task<ActionResult<UserViewmodel>> GetCurrentUser()
        {
            async Task<UserViewmodel> Action()
            {
                UserDto user = ControllerHelper.GetUserModelFromJwtClaims(this);
                UserDto? dbUser = await _userLogic.Find(user.Uuid);
                if (dbUser == null)
                {
                    throw new KeyNotFoundException();
                }

                return dbUser.Adapt<UserViewmodel>();
            }

            return await _controllerResultHelper.Execute(Action()) ?? throw new NoNullAllowedException();
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken()
        {
            async Task Action()
            {
                IPAddress ip = Request.HttpContext.Connection.RemoteIpAddress ?? throw new NoNullAllowedException();
                UserTokensViewmodel userTokens = ControllerHelper.GetUserTokens(this);
                UserTokensViewmodel tokens = await _userLogic.RefreshToken(userTokens, ip);

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
            }

            return await _controllerResultHelper.Execute(Action()) ?? throw new NoNullAllowedException();
        }

        [HttpPost("request-password-reset")]
        public async Task<ActionResult> RequestPasswordReset([FromQuery] string email)
        {
            async Task Action()
            {
                await _userLogic.RequestPasswordReset(email);
            }

            return await _controllerResultHelper.Execute(Action());
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetUserPassword([FromQuery] Guid code, [FromQuery] string newPassword)
        {
            async Task Action()
            {
                await _userLogic.ResetPassword(code, newPassword);
            }

            return await _controllerResultHelper.Execute(Action());
        }

        [HttpPost("activate")]
        public async Task<ActionResult> ActivateUser([FromQuery] Guid code)
        {
            async Task Action()
            {
                await _userLogic.ActivateUserAccount(code);
            }

            return await _controllerResultHelper.Execute(Action());
        }

        [HttpPost("login")]
        public async Task<ActionResult?> Login([FromBody] User user)
        {
            async Task Action()
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
            }

            return await _controllerResultHelper.Execute(Action());
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

                await _userLogic.Update(userDto, user.NewPassword);
            }

            return await _controllerResultHelper.Execute(Action());
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

            return await _controllerResultHelper.Execute(Action());
        }
    }
}
