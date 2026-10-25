using Dotnetstore.BaseModel.Entities;
using Shouldly;

namespace Dotnetstore.BaseModel.Tests.Entities;

public class AuditableTests
{
    private sealed class TestAuditable : Auditable;

    [Fact]
    public void NewInstanceHasExpectedDefaults()
    {
        var sut = new TestAuditable();

        sut.CreatedDate.ShouldBe(default);
        sut.CreatedBy.ShouldBeNull();
        sut.LastUpdatedDate.ShouldBeNull();
        sut.LastUpdatedBy.ShouldBeNull();
        sut.DeletedDate.ShouldBeNull();
        sut.DeletedBy.ShouldBeNull();
        sut.IsDeleted.ShouldBeFalse();
        sut.IsSystem.ShouldBeFalse();
        sut.IsGdpr.ShouldBeFalse();
    }

    [Fact]
    public void PropertiesAreSetThroughInitializer()
    {
        var created = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var updated = created.AddDays(1);
        var deleted = created.AddDays(2);
        var userId = Guid.NewGuid();

        var sut = new TestAuditable
        {
            CreatedDate = created,
            CreatedBy = userId,
            LastUpdatedDate = updated,
            LastUpdatedBy = userId,
            DeletedDate = deleted,
            DeletedBy = userId,
            IsDeleted = true,
            IsSystem = true,
            IsGdpr = true
        };

        sut.CreatedDate.ShouldBe(created);
        sut.CreatedBy.ShouldBe(userId);
        sut.LastUpdatedDate.ShouldBe(updated);
        sut.LastUpdatedBy.ShouldBe(userId);
        sut.DeletedDate.ShouldBe(deleted);
        sut.DeletedBy.ShouldBe(userId);
        sut.IsDeleted.ShouldBeTrue();
        sut.IsSystem.ShouldBeTrue();
        sut.IsGdpr.ShouldBeTrue();
    }
}
