namespace Dotnetstore.BaseModel.Entities;

public abstract class PersonIdentity : Person
{
    public required string Username { get; init; }

    public required string PasswordHash { get; init; }

    public required string SecurityStamp { get; init; }

    public bool EmailAddressConfirmed { get; init; }

    public string? EmailAddressConfirmationToken { get; init; }

    public bool AccountIsApproved { get; init; }

    public bool IsLockedOut { get; init; }

    public DateTime? LockoutEnd { get; init; }

    public int AccessFailedCount { get; init; }

    public DateTime? LastLoginAt { get; init; }
}
