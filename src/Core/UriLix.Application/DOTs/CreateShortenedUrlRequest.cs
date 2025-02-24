using System.ComponentModel.DataAnnotations;

namespace UriLix.Application.DOTs;

public sealed record CreateShortenedUrlRequest(
    Guid? UserId, 
    
    [Required]
    [Url(ErrorMessage = "The URL must be in a valid format.")]
    [StringLength(200, ErrorMessage = "The url must at most 200 characters")]
    string OriginalUrl, 
    
    [MinLength(4, ErrorMessage = "The alias must be at least 5 characters")]
    [MaxLength(20, ErrorMessage = "The alias must be at most 20 characters")]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "The alias can only contain letters and numbers")]
    string? Alias);
