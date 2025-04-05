using UriLix.Domain.Entities;

namespace UriLix.Domain.Repositories;

public interface IClickTrackingRepository
{
    Task<ClickStatistic> InsertAsync(ClickStatistic clickStatistic);
}
