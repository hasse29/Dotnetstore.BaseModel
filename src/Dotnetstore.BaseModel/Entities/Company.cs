namespace Dotnetstore.BaseModel.Entities;

public abstract class Company : Auditable
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public string? Description { get; init; }

    public Uri? LogoUrl { get; init; }

    public Uri? WebsiteUrl { get; init; }

    public string? CorporateNumber { get; init; }

    public string? TaxId { get; init; }
}