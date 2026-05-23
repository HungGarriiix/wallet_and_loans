using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using wallet_and_loans_api.IServices;

namespace wallet_and_loans_api.Common.Attributes
{
    public class UserAuthorizationFilter: IAuthorizationFilter
    {
        private readonly IAuthService _authService;

        public UserAuthorizationFilter(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Implement your user authorization logic here
            // For example, check if the user is authenticated and has the required roles/permissions
            // If unauthorized, you can set the context.Result to an appropriate result
            // context.Result = new UnauthorizedResult();
            string userId = context.HttpContext.Request.Headers["X-Session-Id"];
            int platform = int.Parse(context.HttpContext.Request.Headers["X-Platform-Id"]);

            if (!userId.Any())
            {
                context.Result = new UnauthorizedObjectResult("No");
                return;
            }
            if (!_authService.CheckUserRegistered(userId, platform))
            {
                context.Result = new UnauthorizedObjectResult("No user");
                return;
            }
        }
    }
}
