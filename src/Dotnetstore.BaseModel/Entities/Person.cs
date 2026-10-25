namespace Dotnetstore.BaseModel.Entities;

public abstract class Person : Auditable
{
    public required string LastName { get; init; }

    public required string FirstName { get; init; }

    public string? MiddleName { get; init; }

    public DateTime? DateOfBirth { get; init; }

    public string? SocialSecurityNumber { get; init; }

    public bool IsMale { get; init; }
}