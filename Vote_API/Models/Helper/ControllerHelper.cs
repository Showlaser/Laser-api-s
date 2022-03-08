using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using Vote_API.Logic;

namespace Vote_API.Models.Helper
{
    public static class ControllerHelper
    {
        public static UserModel GetUserModelFromJwtClaims(ControllerBase controllerBase)
        {
            string jwt = controllerBase.ControllerContext.HttpContext.Request.Cookies["jwt"]?.Replace("Bearer ", "") ?? throw new NoNullAllowedException();
            Claim userUuidClaim = JwtLogic.GetJwtClaims(jwt).Single(c => c.Type == "uuid");
            Guid userUuid = Guid.Parse(userUuidClaim.Value);
            return new UserModel
            {
                Uuid = userUuid
            };
        }
    }
}
