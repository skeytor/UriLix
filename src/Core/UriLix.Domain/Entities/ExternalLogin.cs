namespace UriLix.Domain.Entities;

public class ExternalLogin
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public User? User { get; set; } = default!;
    public string Provider { get; set; } = string.Empty;
    public string ProviderKey { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
}
