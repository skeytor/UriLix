using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UriLix.Domain.Entities;
using UriLix.Persistence.Helpers;

namespace UriLix.Persistence.Configurations;

internal sealed class ShortenedLinkConfig : IEntityTypeConfiguration<ShortenedLink>
{
    public void Configure(EntityTypeBuilder<ShortenedLink> builder)
    {
        builder.ToTable(TableName.ShortenedLinks);
        
        builder.HasIndex(x => x.Id);
        
        builder.Property(x => x.OriginalUrl)
            .HasMaxLength(256)
            .IsRequired();
        
        builder.Property(x => x.ShortCode)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Alias)
            .HasMaxLength(5)
            .IsRequired(false);

        builder.Property(x => x.CreateAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.UpdateAt)
            .ValueGeneratedOnUpdate()
            .HasDefaultValueSql("GETDATE()");

        ConfigureRelationships(builder);
    }
    private static void ConfigureRelationships(EntityTypeBuilder<ShortenedLink> builder)
    {
        builder.HasMany(x => x.ClickStatistics)
            .WithOne(x => x.ShortenedLink)
            .HasForeignKey(x => x.ShortenedLinkId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
