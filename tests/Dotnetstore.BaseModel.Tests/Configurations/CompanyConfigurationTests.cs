using Dotnetstore.BaseModel.Configurations;
using Dotnetstore.BaseModel.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Shouldly;

namespace Dotnetstore.BaseModel.Tests.Configurations;

public class CompanyConfigurationTests
{
    private sealed class TestCompany : Company;

    private sealed class TestCompanyConfiguration : CompanyConfiguration<TestCompany>;

    private static readonly IEntityType EntityType =
        ConfigurationTestHelper.BuildEntityType(new TestCompanyConfiguration());

    [Fact]
    public void UsesIdAsPrimaryKey()
    {
        EntityType.FindPrimaryKey()!.Properties.Select(x => x.Name).ShouldBe([nameof(Company.Id)]);
    }

    [Theory]
    [InlineData(nameof(Company.Name), false, 200)]
    [InlineData(nameof(Company.Description), true, 2000)]
    [InlineData(nameof(Company.LogoUrl), true, 2048)]
    [InlineData(nameof(Company.WebsiteUrl), true, 2048)]
    [InlineData(nameof(Company.CorporateNumber), true, 50)]
    [InlineData(nameof(Company.TaxId), true, 50)]
    public void ConfiguresProperty(string propertyName, bool isNullable, int maxLength)
    {
        var property = EntityType.GetProperty(propertyName);

        property.IsNullable.ShouldBe(isNullable);
        property.GetMaxLength().ShouldBe(maxLength);
    }

    [Fact]
    public void AppliesAuditableConfiguration()
    {
        EntityType.HasIndexOn(nameof(Auditable.IsDeleted)).ShouldBeTrue();
    }
}
