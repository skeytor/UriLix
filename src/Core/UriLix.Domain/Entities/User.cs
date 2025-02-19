namespace UriLix.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public DateTime LastLoginAt { get; set; }
}
