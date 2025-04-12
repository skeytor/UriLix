namespace UriLix.Domain.Entities;

/// <summary>
/// Represents a click statistic entity to track the details of each click on a shortened URL.
/// </summary>
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