using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UriLix.Domain.Entities;
using UriLix.Persistence.Helpers;

namespace UriLix.Persistence.Configurations;

internal sealed class ClickStatisticConfig : IEntityTypeConfiguration<ClickStatistic>
{
    public void Configure(EntityTypeBuilder<ClickStatistic> builder)
    {
        builder.ToTable(TableName.ClickStatistics);

        builder.HasIndex(x => x.Id);

        builder.Property(x => x.Device)
            .HasMaxLength(25);

        builder.Property(x => x.Browser)
            .HasMaxLength(20);

        builder.Property(x => x.UserAgent)
            .HasMaxLength(150);

        builder.Property(x => x.Referer)
            .HasMaxLength(150);

        builder.Property(x => x.VisitedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETDATE()");
    }
}
