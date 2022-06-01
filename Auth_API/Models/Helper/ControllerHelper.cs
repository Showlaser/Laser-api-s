using Auth_API.Logic;
using Auth_API.Models.Dto.User;
using Auth_API.Models.ToFrontend;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using System.Security.Claims;

namespace Auth_API.Models.Helper
{
    public static class ControllerHelper
    {
        public static UserTokensViewmodel GetUserTokens(ControllerBase controllerBase)
        {
            return new UserTokensViewmodel
            {
                Jwt = controllerBase.ControllerContext.HttpContext.Request.Cookies["jwt"]?.Replace("Bearer ", ""),
                RefreshToken = controllerBase.ControllerContext.HttpContext.Request.Cookies["refreshToken"]
            };
        }

        public static UserDto GetUserModelFromJwtClaims(ControllerBase controllerBase)
        {
            string jwt = controllerBase.ControllerContext.HttpContext.Request.Cookies["jwt"]?.Replace("Bearer ", "") ?? throw new UnauthorizedAccessException();
            Claim? userUuidClaim = JwtLogic.GetJwtClaims(jwt).FirstOrDefault(c => c.Type == "uuid");
            if (userUuidClaim == null)
            {
                throw new SecurityException();
            }

            Guid userUuid = Guid.Parse(userUuidClaim.Value);
            return new UserDto
            {
                Uuid = userUuid
            };
        }
    }
}
