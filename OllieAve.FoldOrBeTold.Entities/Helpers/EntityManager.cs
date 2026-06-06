using OllieAve.FoldOrBeTold.Entities.Interfaces;

namespace OllieAve.FoldOrBeTold.Entities.Helpers;

public class EntityManager
{
    private readonly List<IEntity> entities;

    public EntityManager(List<IEntity> entities)
    {
        this.entities = entities;

    }

    public void AddEntity(IEntity entity)
    {
        entities.Add(entity);
    }

    public List<IEntity> GetEntitites()
    {
        return entities;
    }

    public List<IRenderable> GetRenderables()
    {
        return [.. entities.OfType<IRenderable>().OrderByDescending(x => x.RenderingOrder)];
    }

    public Player GetPlayer()
    {
        return entities.SingleOrDefault(x => x is Player) as Player
            ?? throw new Exception("No player found");
    }

    public CameraHandler GetCamera()
    {
        return entities.SingleOrDefault(x => x is CameraHandler) as CameraHandler
            ?? throw new Exception("No camera manager found");
    }
}
