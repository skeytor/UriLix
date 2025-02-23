namespace UriLix.Domain.Entities;

public class ShortenedLink
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public User? User { get; set; } = default;
    public string OriginalUrl { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public string? Alias { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public ICollection<ClickStatistic> ClickStatistics { get; set; } = [];
}