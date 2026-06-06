namespace OllieAve.FoldOrBeTold.Entities;

public abstract class EntityBase
{
    private readonly Guid entityId = Guid.NewGuid();

    public Guid EntityId => entityId;
}
