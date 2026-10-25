using Dotnetstore.BaseModel.Entities;
using Shouldly;

namespace Dotnetstore.BaseModel.Tests.Entities;

public class CompanyTests
{
    private sealed class TestCompany : Company;

    [Fact]
    public void IsAuditable()
    {
        new TestCompany { Name = "Acme" }.ShouldBeAssignableTo<Auditable>();
    }

    [Fact]
    public void NewInstanceHasExpectedDefaults()
    {
        var sut = new TestCompany { Name = "Acme" };

        sut.Id.ShouldBe(Guid.Empty);
        sut.Name.ShouldBe("Acme");
        sut.Description.ShouldBeNull();
        sut.LogoUrl.ShouldBeNull();
        sut.WebsiteUrl.ShouldBeNull();
        sut.CorporateNumber.ShouldBeNull();
        sut.TaxId.ShouldBeNull();
    }

    [Fact]
    public void PropertiesAreSetThroughInitializer()
    {
        var id = Guid.NewGuid();
        var logo = new Uri("https://example.com/logo.png");
        var website = new Uri("https://example.com");

        var sut = new TestCompany
        {
            Id = id,
            Name = "Acme",
            Description = "Anvils",
            LogoUrl = logo,
            WebsiteUrl = website,
            CorporateNumber = "556000-0000",
            TaxId = "SE556000000001"
        };

        sut.Id.ShouldBe(id);
        sut.Description.ShouldBe("Anvils");
        sut.LogoUrl.ShouldBe(logo);
        sut.WebsiteUrl.ShouldBe(website);
        sut.CorporateNumber.ShouldBe("556000-0000");
        sut.TaxId.ShouldBe("SE556000000001");
    }
}
