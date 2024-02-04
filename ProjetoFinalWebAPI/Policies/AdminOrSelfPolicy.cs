using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Policies
{
    public class AdminOrSelfRequirement : IAuthorizationRequirement
    {
    }
    public class AdminOrSelfHandler : AuthorizationHandler<AdminOrSelfRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminOrSelfRequirement requirement)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var isAdmin = context.User.FindFirst(ClaimTypes.Role)!.Value.ToString();

            if (userId != null && isAdmin != null && (int.Parse(userId) == (int)context.Resource! || isAdmin == "Admin"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
