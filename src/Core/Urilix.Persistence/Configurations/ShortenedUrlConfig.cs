using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UriLix.Domain.Entities;
using UriLix.Persistence.Helpers;

namespace UriLix.Persistence.Configurations;

internal sealed class ShortenedUrlConfig : IEntityTypeConfiguration<ShortenedUrl>
{
    public void Configure(EntityTypeBuilder<ShortenedUrl> builder)
    {
        builder.ToTable(TableName.ShortenedUrl);
        
        builder.HasIndex(x => x.Id);
        
        builder.Property(x => x.OriginalUrl)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.ShortCode)
            .HasMaxLength(20);

        builder.Property(x => x.CreateAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.UpdateAt)
            .ValueGeneratedOnUpdate()
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(x => x.ShortCode)
            .IsUnique();

        ConfigureRelationships(builder);
    }
    private static void ConfigureRelationships(EntityTypeBuilder<ShortenedUrl> builder)
    {
        builder.HasMany(x => x.ClickStatistics)
            .WithOne(x => x.ShortenedUrl)
            .HasForeignKey(x => x.ShortenedUrlId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
