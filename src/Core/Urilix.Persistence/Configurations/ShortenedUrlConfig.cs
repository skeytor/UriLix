using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UriLix.Domain.Entities;
using UriLix.Persistence.Helpers;

namespace UriLix.Persistence.Configurations;

internal sealed class ShortenedUrlConfig : IEntityTypeConfiguration<ShortenedUrl>
{
    public void Configure(EntityTypeBuilder<ShortenedUrl> builder)
    {
        builder.ToTable(TableName.ShortenedURLs);
        
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
            .ValueGeneratedOnAdd();
    }
}
