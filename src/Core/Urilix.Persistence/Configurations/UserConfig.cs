using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UriLix.Domain.Entities;
using UriLix.Persistence.Helpers;

namespace UriLix.Persistence.Configurations;

internal sealed class UserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable(TableName.Users);

        builder.Property(x => x.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.CreateAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.UpdateAt)
            .ValueGeneratedOnUpdate()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.LastLoginAt)
            .IsRequired(false);

        ConfigureRelationships(builder);
    }
    private static void ConfigureRelationships(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasMany(x => x.ShortenedURLs)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
