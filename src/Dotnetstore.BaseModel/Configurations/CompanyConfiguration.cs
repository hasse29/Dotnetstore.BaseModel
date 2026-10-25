using Dotnetstore.BaseModel.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dotnetstore.BaseModel.Configurations;

public abstract class CompanyConfiguration<T> : AuditableConfiguration<T>
    where T : Company
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.Configure(builder);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.LogoUrl).HasMaxLength(2048);
        builder.Property(x => x.WebsiteUrl).HasMaxLength(2048);
        builder.Property(x => x.CorporateNumber).HasMaxLength(50);
        builder.Property(x => x.TaxId).HasMaxLength(50);
    }
}
