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
            .HasMaxLength(5)
            .IsRequired(false);

        builder.Property(x => x.Alias)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(x => x.CreateAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.UpdateAt)
            .ValueGeneratedOnUpdate()
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(x => x.ShortCode)
            .IsUnique()
            .HasFilter("[ShortCode] IS NOT NULL AND [ShortCode] <> ''");

        builder.HasIndex(x => x.Alias)
            .IsUnique()
            .HasFilter("[Alias] IS NOT NULL AND [Alias] <> ''");

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
