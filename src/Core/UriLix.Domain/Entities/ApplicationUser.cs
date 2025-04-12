using Microsoft.AspNetCore.Identity;

namespace UriLix.Domain.Entities;
/// <summary>
/// Represents a user in the application. It extends the <see cref="IdentityUser"/> class to include additional properties.
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public ICollection<ShortenedUrl> ShortenedURLs { get; set; } = [];
}
