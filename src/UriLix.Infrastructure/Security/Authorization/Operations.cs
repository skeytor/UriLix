using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace UriLix.Infrastructure.Security.Authorization;

internal static class Operations
{
    internal static OperationAuthorizationRequirement Create = new() { Name = nameof(Create) };
    internal static OperationAuthorizationRequirement Read = new() { Name = nameof(Read) };
    internal static OperationAuthorizationRequirement Update = new() { Name = nameof(Update) };
    internal static OperationAuthorizationRequirement Delete = new() { Name = nameof(Delete) };
}
