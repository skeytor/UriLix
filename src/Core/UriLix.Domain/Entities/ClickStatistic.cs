namespace UriLix.Domain.Entities;

public class ClickStatistic
{
    public int Id { get; set; }
    public Guid ShortenedUrlId { get; set; }
    public ShortenedUrl ShortenedUrl { get; set; } = default!;
    public string Device { get; set; } = string.Empty;
    public string Browser { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string Referer { get; set; } = string.Empty;
    public DateTime VisitedAt { get; set; }
}