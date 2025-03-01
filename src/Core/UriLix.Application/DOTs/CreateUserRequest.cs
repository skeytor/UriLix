using System.ComponentModel.DataAnnotations;

namespace UriLix.Application.DOTs;

public sealed record CreateUserRequest(
    [Required, StringLength(50)] string FirstName,
    [Required, StringLength(50)] string LastName,
    [Required, EmailAddress, StringLength(100)] string Email,
    [Required, DataType(DataType.Password)] string Password);
