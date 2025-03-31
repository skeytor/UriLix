using System.ComponentModel.DataAnnotations;

namespace UriLix.Application.DOTs;

public sealed record LoginRequest(
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    string Email,

    [Required]
    [DataType(DataType.Password)] 
    string Password);
