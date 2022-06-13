using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using Vote_API.Logic;

namespace Vote_API.Models.Helper
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

            string? jwt = context.HttpContext.Request.Cookies["jwt"]?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(jwt))
            {
                Console.WriteLine("JWT EMPTY");
                Debug.WriteLine("JWT EMPTY");
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
