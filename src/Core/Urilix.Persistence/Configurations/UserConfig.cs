using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UriLix.Domain.Entities;
using UriLix.Persistence.Helpers;

namespace UriLix.Persistence.Configurations;

internal sealed class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableName.Users);

        builder.HasIndex(x => x.Id);

        builder.Property(x => x.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Password)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.CreateAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.UpdateAt)
            .ValueGeneratedOnUpdate()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.LastLoginAt)
            .IsRequired(false);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        ConfigureRelationships(builder);
    }
    private static void ConfigureRelationships(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(x => x.ShortenedURLs)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
