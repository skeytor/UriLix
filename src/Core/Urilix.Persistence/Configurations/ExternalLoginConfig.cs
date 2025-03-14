using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UriLix.Domain.Entities;
using UriLix.Persistence.Helpers;

namespace UriLix.Persistence.Configurations;

internal sealed class ExternalLoginConfig : IEntityTypeConfiguration<ExternalLogin>
{
    public void Configure(EntityTypeBuilder<ExternalLogin> builder)
    {
        builder.ToTable(TableName.ExternalLogins);

        builder.HasIndex(x => x.Id);

        builder.Property(x => x.Provider)
            .HasMaxLength(50);

        builder.Property(x => x.ProviderKey)
            .HasMaxLength(100);

        builder.Property(x => x.DisplayName)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(x => x.AccessToken)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.RefreshToken)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.CreateAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(x => new { x.Provider, x.ProviderKey })
            .IsUnique();

        ConfigureRelationships(builder);
    }
    private static void ConfigureRelationships(EntityTypeBuilder<ExternalLogin> builder)
    {
        builder.HasOne(x => x.User)
            .WithOne(x => x.ExternalLogin)
            .HasForeignKey<ExternalLogin>(x => x.UserId)
            .IsRequired(false);
    }
}
