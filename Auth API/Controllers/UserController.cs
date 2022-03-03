using Auth_API.Logic;
using Auth_API.Models.Dto.User;
using Auth_API.Models.FromFrontend.User;
using Auth_API.Models.Helper;
using Mapster;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromForm] User user)
        {
            async Task<string> Action()
            {
                UserDto userDto = user.Adapt<UserDto>();
                return await _userLogic.Login(userDto);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            string? jwt = await controllerErrorHandler.Execute(Action());
            //TODO set cookie secure on true
            CookieOptions cookieOptions = new()
            {
                HttpOnly = true,
                Secure = false,
                Path = "/",
                Expires = DateTime.Now.AddDays(7)
            };

            Response.Cookies.Append("jwt", jwt, cookieOptions);
            return Ok();
        }

        [AuthorizedAction]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] User user)
        {
            async Task Action()
            {
                UserDto userDto = user.Adapt<UserDto>();
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
