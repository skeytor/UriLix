using System.ComponentModel.DataAnnotations;

namespace UriLix.Application.DOTs;

/// <summary>
/// Represents a request to create a new user account with required personal and authentication information.
/// </summary>
/// <param name="FirstName">User's first name</param>
/// <param name="LastName">User's last name</param>
/// <param name="Email">User's email</param>
/// <param name="Password">
/// User's password.
/// Required field, must meet password complexity requirements.
/// The actual requirements are determined by the password validation policy.
/// </param>
public sealed record CreateUserRequest(
    [Required, StringLength(50)] 
    string FirstName,
    
    [Required, StringLength(50)] 
    string LastName,

    [Required, EmailAddress, StringLength(100)] 
    string Email,

    [Required, DataType(DataType.Password)] 
    string Password);
