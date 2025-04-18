namespace UriLix.Application.DOTs;

/// <summary>
/// Represents a user profile response containing personal identification information.
/// </summary>
/// <param name="FullName">The user's full name</param>
/// <param name="Email">The user's email</param>
public sealed record UserProfileResponse(string FullName, string Email);