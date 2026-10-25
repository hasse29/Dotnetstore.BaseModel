using Dotnetstore.BaseModel.Entities;
using Shouldly;

namespace Dotnetstore.BaseModel.Tests.Entities;

public class PersonIdentityTests
{
    private sealed class TestPersonIdentity : PersonIdentity;

    private static TestPersonIdentity Create() => new()
    {
        FirstName = "Ada",
        LastName = "Lovelace",
        Username = "ada@example.com",
        PasswordHash = "hash",
        SecurityStamp = "stamp"
    };

    [Fact]
    public void IsPerson()
    {
        Create().ShouldBeAssignableTo<Person>();
    }

    [Fact]
    public void NewInstanceHasExpectedDefaults()
    {
        var sut = Create();

        sut.Username.ShouldBe("ada@example.com");
        sut.PasswordHash.ShouldBe("hash");
        sut.SecurityStamp.ShouldBe("stamp");
        sut.EmailAddressConfirmed.ShouldBeFalse();
        sut.EmailAddressConfirmationToken.ShouldBeNull();
        sut.AccountIsApproved.ShouldBeFalse();
        sut.IsLockedOut.ShouldBeFalse();
        sut.LockoutEnd.ShouldBeNull();
        sut.AccessFailedCount.ShouldBe(0);
        sut.LastLoginAt.ShouldBeNull();
    }

    [Fact]
    public void PropertiesAreSetThroughInitializer()
    {
        var lockoutEnd = new DateTime(2026, 5, 1, 12, 0, 0, DateTimeKind.Utc);
        var lastLogin = lockoutEnd.AddDays(-1);

        var sut = new TestPersonIdentity
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            Username = "ada@example.com",
            PasswordHash = "hash",
            SecurityStamp = "stamp",
            EmailAddressConfirmed = true,
            EmailAddressConfirmationToken = "token",
            AccountIsApproved = true,
            IsLockedOut = true,
            LockoutEnd = lockoutEnd,
            AccessFailedCount = 5,
            LastLoginAt = lastLogin
        };

        sut.EmailAddressConfirmed.ShouldBeTrue();
        sut.EmailAddressConfirmationToken.ShouldBe("token");
        sut.AccountIsApproved.ShouldBeTrue();
        sut.IsLockedOut.ShouldBeTrue();
        sut.LockoutEnd.ShouldBe(lockoutEnd);
        sut.AccessFailedCount.ShouldBe(5);
        sut.LastLoginAt.ShouldBe(lastLogin);
    }
}
