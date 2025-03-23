using System.ComponentModel.DataAnnotations;

namespace UriLix.Application.DOTs;

public sealed record LoginRequest(
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    string Email,
    
    [DataType(DataType.Password)] 
    string Password);
