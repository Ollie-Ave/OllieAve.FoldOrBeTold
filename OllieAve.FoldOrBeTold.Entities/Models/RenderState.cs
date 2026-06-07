using OllieAve.FoldOrBeTold.Entities.Helpers;

namespace OllieAve.FoldOrBeTold.Entities.Models;

public record RenderState
{
    public required EntityManager EntityManager { get; init; }
}
