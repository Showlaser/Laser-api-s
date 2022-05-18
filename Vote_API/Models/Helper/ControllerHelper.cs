using Microsoft.AspNetCore.Mvc;
using System.Security;
using System.Security.Claims;
using Vote_API.Logic;

namespace Vote_API.Models.Helper
{
    public static class ControllerHelper
    {
        public static UserModel GetUserModelFromJwtClaims(ControllerBase controllerBase)
        {
            if (!controllerBase.ControllerContext.HttpContext.Request.Cookies.Any())
            {
                throw new SecurityException();
            }

            string jwtCookieString = controllerBase.ControllerContext.HttpContext.Request.Cookies["jwt"] ?? throw new SecurityException();
            string jwt = jwtCookieString.Replace("Bearer ", "");
            Claim userUuidClaim = JwtLogic.GetJwtClaims(jwt).Single(c => c.Type == "uuid");
            Guid userUuid = Guid.Parse(userUuidClaim.Value);
            return new UserModel
            {
                Uuid = userUuid
            };
        }
    }
}
