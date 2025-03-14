namespace UriLix.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public string? Password { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public ICollection<ShortenedUrl> ShortenedLinks { get; set; } = [];
    public ExternalLogin? ExternalLogin { get; set; } = default!;
}
