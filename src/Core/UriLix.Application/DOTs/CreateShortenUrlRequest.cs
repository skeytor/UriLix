using System.ComponentModel.DataAnnotations;

namespace UriLix.Application.DOTs;

/// <summary>
/// Represents a request to create a shortened URL, including optional custom alias validation.
/// </summary>
/// <param name="OriginalUrl">The original URL to be shortened</param>
/// <param name="Alias">Optional custom alias for the shortened URL</param>
public sealed record CreateShortenUrlRequest(
    [Required]
    [Url(ErrorMessage = "The URL must be in a valid format.")]
    [StringLength(200, ErrorMessage = "The url must at most 200 characters")]
    string OriginalUrl, 

    [MinLength(4, ErrorMessage = "The alias must be at least 5 characters")]
    [MaxLength(20, ErrorMessage = "The alias must be at most 20 characters")]
    [RegularExpression("^[a-zA-Z0-9-]*$", ErrorMessage = "The alias can only contain letters and numbers")]
    string? Alias = null);
