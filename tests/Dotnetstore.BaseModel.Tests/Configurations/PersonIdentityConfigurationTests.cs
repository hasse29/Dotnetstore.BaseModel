using Dotnetstore.BaseModel.Configurations;
using Dotnetstore.BaseModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shouldly;

namespace Dotnetstore.BaseModel.Tests.Configurations;

public class PersonIdentityConfigurationTests
{
    private sealed class TestPersonIdentity : PersonIdentity
    {
        public Guid Id { get; init; }
    }

    private sealed class TestPersonIdentityConfiguration : PersonIdentityConfiguration<TestPersonIdentity>
    {
        public override void Configure(EntityTypeBuilder<TestPersonIdentity> builder)
        {
            base.Configure(builder);
            builder.HasKey(x => x.Id);
        }
    }

    private static readonly IEntityType EntityType =
        ConfigurationTestHelper.BuildEntityType(new TestPersonIdentityConfiguration());

    [Theory]
    [InlineData(nameof(PersonIdentity.Username), false, 256)]
    [InlineData(nameof(PersonIdentity.PasswordHash), false, 512)]
    [InlineData(nameof(PersonIdentity.SecurityStamp), false, 256)]
    [InlineData(nameof(PersonIdentity.EmailAddressConfirmationToken), true, 256)]
    public void ConfiguresStringProperty(string propertyName, bool isNullable, int maxLength)
    {
        var property = EntityType.GetProperty(propertyName);

        property.IsNullable.ShouldBe(isNullable);
        property.GetMaxLength().ShouldBe(maxLength);
    }

    [Theory]
    [InlineData(nameof(PersonIdentity.EmailAddressConfirmed), false)]
    [InlineData(nameof(PersonIdentity.AccountIsApproved), false)]
    [InlineData(nameof(PersonIdentity.IsLockedOut), false)]
    [InlineData(nameof(PersonIdentity.LockoutEnd), true)]
    [InlineData(nameof(PersonIdentity.AccessFailedCount), false)]
    [InlineData(nameof(PersonIdentity.LastLoginAt), true)]
    public void ConfiguresNullability(string propertyName, bool isNullable)
    {
        EntityType.GetProperty(propertyName).IsNullable.ShouldBe(isNullable);
    }

    [Fact]
    public void AddsUniqueIndexOnUsername()
    {
        var index = EntityType.GetIndexes()
            .SingleOrDefault(x => x.Properties.Count == 1 && x.Properties[0].Name == nameof(PersonIdentity.Username));

        index.ShouldNotBeNull();
        index.IsUnique.ShouldBeTrue();
    }

    [Fact]
    public void AppliesPersonAndAuditableConfiguration()
    {
        EntityType.GetProperty(nameof(Person.FirstName)).GetMaxLength().ShouldBe(25);
        EntityType.HasIndexOn(nameof(Auditable.IsDeleted)).ShouldBeTrue();
    }
}
