using UriLix.Domain.Entities;

namespace UriLix.Domain.Repositories;

/// <summary>
/// Interface for the Click Tracking Repository.
/// </summary>
public interface IClickTrackingRepository
{
    /// <summary>
    /// Inserts a new click statistic into the database.
    /// </summary>
    /// <param name="clickStatistic"></param>
    /// <returns></returns>
    Task<ClickStatistic> InsertAsync(ClickStatistic clickStatistic);
}
