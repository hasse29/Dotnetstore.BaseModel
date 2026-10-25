using Dotnetstore.BaseModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dotnetstore.BaseModel.Configurations;

public abstract class AuditableConfiguration<T> : IEntityTypeConfiguration<T>
    where T : Auditable
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Property(x => x.CreatedDate).IsRequired();
        builder.Property(x => x.CreatedBy);
        builder.Property(x => x.LastUpdatedDate);
        builder.Property(x => x.LastUpdatedBy);
        builder.Property(x => x.DeletedDate);
        builder.Property(x => x.DeletedBy);
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.IsSystem).IsRequired();
        builder.Property(x => x.IsGdpr).IsRequired();

        builder.HasIndex(x => x.IsDeleted);
    }
}
