using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;

namespace UriLix.Persistence.Repositories;

public class ClickTrackingRepository(ApplicationDbContext context) 
    : BaseRepository(context), IClickTrackingRepository
{
    public async Task<ClickStatistic> InsertAsync(ClickStatistic clickStatistic)
    {
        await Context.ClickStatistics.AddAsync(clickStatistic);
        return clickStatistic;
    }
}
