using OllieAve.FoldOrBeTold.Entities.Helpers;

namespace OllieAve.FoldOrBeTold.Entities.Models;

public record UpdateState
{
    public required EntityManager EntityManager { get; init; }
}
