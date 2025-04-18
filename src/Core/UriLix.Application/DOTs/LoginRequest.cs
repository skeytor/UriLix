using System.ComponentModel.DataAnnotations;

namespace UriLix.Application.DOTs;

/// <summary>
/// Represents a user login request containing authentication credentials.
/// </summary>
/// <param name="Email"></param>
/// <param name="Password"></param>
public sealed record LoginRequest(
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    string Email,

    [Required]
    [DataType(DataType.Password)] 
    string Password);
