namespace UriLix.Domain.Entities;

public class ClickStatistic
{
    public int Id { get; set; }
    public Guid ShortenedLinkId { get; set; }
    public ShortenedLink ShortenedLink { get; set; } = null!;
    public string IpAddress { get; set; } = string.Empty;
    public string Device { get; set; } = string.Empty;
    public string Country { set; get; } = string.Empty;
    public string Browser { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string Referrer { get; set; } = string.Empty;
    public DateTime VisitedAt { get; set; }
}
