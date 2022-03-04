using Auth_API.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Auth_API.Models.Helper
{
    public class AuthorizedAction : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool allowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

            if (allowAnonymous) // skip authorization if allow anonymous attribute is used
            {
                return;
            }

            string jwt = context.HttpContext.Request.Cookies["jwt"].Replace("Bearer ", "");
            if (string.IsNullOrEmpty(jwt))
            {
                context.Result = new UnauthorizedResult();
                base.OnActionExecuting(context);
                return;
            }

            bool jwtValid = JwtLogic.ValidateJwtToken(jwt);
            if (!jwtValid)
            {
                context.Result = new UnauthorizedResult();
            }

            base.OnActionExecuting(context);
        }
    }
}
