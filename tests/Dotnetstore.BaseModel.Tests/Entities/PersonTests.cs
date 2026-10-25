using Dotnetstore.BaseModel.Entities;
using Shouldly;

namespace Dotnetstore.BaseModel.Tests.Entities;

public class PersonTests
{
    private sealed class TestPerson : Person;

    [Fact]
    public void IsAuditable()
    {
        new TestPerson { FirstName = "Ada", LastName = "Lovelace" }.ShouldBeAssignableTo<Auditable>();
    }

    [Fact]
    public void NewInstanceHasExpectedDefaults()
    {
        var sut = new TestPerson { FirstName = "Ada", LastName = "Lovelace" };

        sut.FirstName.ShouldBe("Ada");
        sut.LastName.ShouldBe("Lovelace");
        sut.MiddleName.ShouldBeNull();
        sut.DateOfBirth.ShouldBeNull();
        sut.SocialSecurityNumber.ShouldBeNull();
        sut.IsMale.ShouldBeFalse();
    }

    [Fact]
    public void PropertiesAreSetThroughInitializer()
    {
        var birth = new DateTime(1815, 12, 10);

        var sut = new TestPerson
        {
            FirstName = "Ada",
            MiddleName = "Augusta",
            LastName = "Lovelace",
            DateOfBirth = birth,
            SocialSecurityNumber = "181512100000",
            IsMale = false
        };

        sut.MiddleName.ShouldBe("Augusta");
        sut.DateOfBirth.ShouldBe(birth);
        sut.SocialSecurityNumber.ShouldBe("181512100000");
    }
}
