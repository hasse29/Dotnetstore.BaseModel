using Dotnetstore.BaseModel.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dotnetstore.BaseModel.Configurations;

public abstract class PersonIdentityConfiguration<T> : PersonConfiguration<T>
    where T : PersonIdentity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.Configure(builder);

        builder.Property(x => x.Username).IsRequired().HasMaxLength(256);
        builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(512);
        builder.Property(x => x.SecurityStamp).IsRequired().HasMaxLength(256);
        builder.Property(x => x.EmailAddressConfirmed).IsRequired();
        builder.Property(x => x.EmailAddressConfirmationToken).HasMaxLength(256);
        builder.Property(x => x.AccountIsApproved).IsRequired();
        builder.Property(x => x.IsLockedOut).IsRequired();
        builder.Property(x => x.LockoutEnd);
        builder.Property(x => x.AccessFailedCount).IsRequired();
        builder.Property(x => x.LastLoginAt);

        builder.HasIndex(x => x.Username).IsUnique();
    }
}
