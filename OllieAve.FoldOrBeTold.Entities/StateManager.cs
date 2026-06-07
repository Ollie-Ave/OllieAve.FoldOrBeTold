using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities;

public class StateManager : EntityBase, IEntity
{
    private const float totalTimerSeconds = 2 * 60;
    private float secondsRemaining;
    private bool debugMode = true;

    public StateManager()
    {
        secondsRemaining = totalTimerSeconds;
    }

    public float GetPercentageOfTimeRemaining()
    {
        return secondsRemaining / totalTimerSeconds * 100f;
    }

    public void Update(UpdateState updateState)
    {
        secondsRemaining -= Raylib.GetFrameTime();

        if (Raylib.IsKeyPressed(KeyboardKey.F3))
            debugMode = !debugMode;
    }

    public bool IsDebugMode()
    {
        return debugMode;
    }

    public float GeSecondsRemaining()
    {
        return secondsRemaining;
    }
}
