using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApi.Requrements
{
    public class KnownMailHandler : AuthorizationHandler<KnownMailRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, KnownMailRequirement requirement)
        {
            var email = context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            if(email?.EndsWith(requirement.Domain) ?? false)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
