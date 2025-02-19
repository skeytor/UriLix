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
        builder.Property(x => x.UserName)
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(x => x.Email)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x => x.Password)
            .HasMaxLength(200)
            .IsRequired();
    }
}
