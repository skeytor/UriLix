using Microsoft.AspNetCore.Identity;

namespace UriLix.Domain.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public ICollection<ShortenedUrl> ShortenedURLs { get; set; } = [];
}
