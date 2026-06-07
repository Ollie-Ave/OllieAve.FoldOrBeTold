using OllieAve.FoldOrBeTold.Entities.Interfaces;

namespace OllieAve.FoldOrBeTold.Entities.Helpers;

public class EntityManager
{
    private readonly Dictionary<Guid, IEntity> entities;

    public EntityManager(List<IEntity> entities)
    {
        this.entities = entities.ToDictionary(x => x.EntityId, x => x);

    }

    public void SpawnEntity(IEntity entity)
    {
        entities.Add(entity.EntityId, entity);
    }

    public void DespawnEntity(Guid entityId)
    {
        entities.Remove(entityId);
    }

    public List<IEntity> GetEntitites()
    {
        return [.. entities.Select(x => x.Value)];
    }


    public List<IRenderable> GetRenderables()
    {
        return [.. entities.Select(x => x.Value).OfType<IRenderable>().OrderByDescending(x => x.RenderingOrder)];
    }

    public List<IUiRenderable> GetUiRenderables()
    {
        return [.. entities.Select(x => x.Value).OfType<IUiRenderable>()];
    }

    public Player GetPlayer()
    {
        return entities.Select(x => x.Value).SingleOrDefault(x => x is Player) as Player
            ?? throw new Exception("No player found");
    }

    public CameraHandler GetCamera()
    {
        return entities.Select(x => x.Value).SingleOrDefault(x => x is CameraHandler) as CameraHandler
            ?? throw new Exception("No camera manager found");
    }

    public StateManager GetStateManager()
    {
        return entities.Select(x => x.Value).SingleOrDefault(x => x is StateManager) as StateManager
            ?? throw new Exception("No state manager found");
    }
}
