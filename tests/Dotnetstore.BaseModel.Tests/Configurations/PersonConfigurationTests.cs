using Dotnetstore.BaseModel.Configurations;
using Dotnetstore.BaseModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shouldly;

namespace Dotnetstore.BaseModel.Tests.Configurations;

public class PersonConfigurationTests
{
    private sealed class TestPerson : Person
    {
        public Guid Id { get; init; }
    }

    private sealed class TestPersonConfiguration : PersonConfiguration<TestPerson>
    {
        public override void Configure(EntityTypeBuilder<TestPerson> builder)
        {
            base.Configure(builder);
            builder.HasKey(x => x.Id);
        }
    }

    private static readonly IEntityType EntityType =
        ConfigurationTestHelper.BuildEntityType(new TestPersonConfiguration());

    [Theory]
    [InlineData(nameof(Person.FirstName), false, 25)]
    [InlineData(nameof(Person.MiddleName), true, 25)]
    [InlineData(nameof(Person.LastName), false, 25)]
    [InlineData(nameof(Person.SocialSecurityNumber), true, 50)]
    public void ConfiguresStringProperty(string propertyName, bool isNullable, int maxLength)
    {
        var property = EntityType.GetProperty(propertyName);

        property.IsNullable.ShouldBe(isNullable);
        property.GetMaxLength().ShouldBe(maxLength);
    }

    [Theory]
    [InlineData(nameof(Person.DateOfBirth), true)]
    [InlineData(nameof(Person.IsMale), false)]
    public void ConfiguresNullability(string propertyName, bool isNullable)
    {
        EntityType.GetProperty(propertyName).IsNullable.ShouldBe(isNullable);
    }

    [Fact]
    public void AppliesAuditableConfiguration()
    {
        EntityType.HasIndexOn(nameof(Auditable.IsDeleted)).ShouldBeTrue();
    }
}
