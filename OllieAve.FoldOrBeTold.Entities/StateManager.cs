using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities;

public class StateManager : EntityBase, IEntity
{
    private float secondsRemaining;

    public StateManager()
    {
        secondsRemaining = 2 * 60;
    }

    public void Update(UpdateState updateState)
    {
        secondsRemaining -= Raylib.GetFrameTime();
    }

    public float GeSecondsRemaining()
    {
        return secondsRemaining;
    }
}
