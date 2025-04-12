namespace UriLix.Domain.Entities;

/// <summary>
/// Represents a shortened URL entity.
/// </summary>
public class ShortenedUrl
{
    public Guid Id { get; set; }
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; } = default;
    public string OriginalUrl { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public ICollection<ClickStatistic> ClickStatistics { get; set; } = [];
}