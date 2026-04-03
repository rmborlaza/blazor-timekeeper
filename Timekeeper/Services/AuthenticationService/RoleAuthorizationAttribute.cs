using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Timekeeper.Models;

namespace Timekeeper.Services.AuthenticationService
{
    public class RoleAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private AccountType _type;
        public RoleAuthorizationAttribute(AccountType type)
        {
            _type = type;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity!.IsAuthenticated || !user.IsInRole(_type.ToString()))
            {
                context.HttpContext.Response.StatusCode = 403;
                context.Result = new ForbidResult();
            }
        }
    }
}
