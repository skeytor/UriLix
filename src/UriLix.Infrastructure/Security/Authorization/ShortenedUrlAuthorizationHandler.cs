using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UriLix.Domain.Entities;

namespace UriLix.Infrastructure.Security.Authorization;

internal sealed class ShortenedUrlAuthorizationHandler : AuthorizationHandler<SameUserRequirement, ShortenedUrl>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        SameUserRequirement requirement, 
        ShortenedUrl resource)
    {
        string? userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Task.CompletedTask;
        }
        if (resource.UserId == userId)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}
internal class SameUserRequirement : IAuthorizationRequirement;
