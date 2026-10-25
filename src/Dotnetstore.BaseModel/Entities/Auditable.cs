namespace Dotnetstore.BaseModel.Entities;

public abstract class Auditable
{
    public DateTime CreatedDate { get; init; }

    public Guid? CreatedBy { get; init; }

    public DateTime? LastUpdatedDate { get; init; }
    
    public Guid? LastUpdatedBy { get; init; }

    public DateTime? DeletedDate { get; init; }
    
    public Guid? DeletedBy { get; init; }

    public bool IsDeleted { get; init; }

    public bool IsSystem { get; init; }

    public bool IsGdpr { get; init; }
}