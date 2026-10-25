using Dotnetstore.BaseModel.Configurations;
using Dotnetstore.BaseModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shouldly;

namespace Dotnetstore.BaseModel.Tests.Configurations;

public class AuditableConfigurationTests
{
    private sealed class TestAuditable : Auditable
    {
        public Guid Id { get; init; }
    }

    private sealed class TestAuditableConfiguration : AuditableConfiguration<TestAuditable>
    {
        public override void Configure(EntityTypeBuilder<TestAuditable> builder)
        {
            base.Configure(builder);
            builder.HasKey(x => x.Id);
        }
    }

    private static readonly IEntityType EntityType =
        ConfigurationTestHelper.BuildEntityType(new TestAuditableConfiguration());

    [Theory]
    [InlineData(nameof(Auditable.CreatedDate), false)]
    [InlineData(nameof(Auditable.CreatedBy), true)]
    [InlineData(nameof(Auditable.LastUpdatedDate), true)]
    [InlineData(nameof(Auditable.LastUpdatedBy), true)]
    [InlineData(nameof(Auditable.DeletedDate), true)]
    [InlineData(nameof(Auditable.DeletedBy), true)]
    [InlineData(nameof(Auditable.IsDeleted), false)]
    [InlineData(nameof(Auditable.IsSystem), false)]
    [InlineData(nameof(Auditable.IsGdpr), false)]
    public void ConfiguresNullability(string propertyName, bool isNullable)
    {
        EntityType.GetProperty(propertyName).IsNullable.ShouldBe(isNullable);
    }

    [Fact]
    public void AddsIndexOnIsDeleted()
    {
        EntityType.HasIndexOn(nameof(Auditable.IsDeleted)).ShouldBeTrue();
    }
}
