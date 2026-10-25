using Dotnetstore.BaseModel.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dotnetstore.BaseModel.Configurations;

public abstract class PersonConfiguration<T> : AuditableConfiguration<T>
    where T : Person
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.Configure(builder);

        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(25);
        builder.Property(x => x.MiddleName).HasMaxLength(25);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(25);
        builder.Property(x => x.DateOfBirth);
        builder.Property(x => x.SocialSecurityNumber).HasMaxLength(50);
        builder.Property(x => x.IsMale).IsRequired();
    }
}
