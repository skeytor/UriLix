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

        builder.Property(x => x.IpAddress)
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(x => x.Device)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Country)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Browser)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.UserAgent)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.UserAgent)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Referrer)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.VisitedAt)
            .IsRequired();
    }
}
