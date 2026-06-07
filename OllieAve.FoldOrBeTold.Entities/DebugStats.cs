using System.Numerics;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities;

public class DebugStats : EntityBase, IEntity, IUiRenderable
{
    private const int fontSize = 16;

    public void Render(RenderState state)
    {
        if (!state.EntityManager.GetStateManager().IsDebugMode())
            return;

        Vector2 playerPosition = state.EntityManager.GetPlayer().GetPosition();
        string debugText = $"Player - X: {playerPosition.X}, Y: {playerPosition.Y}";
        Raylib.DrawText(debugText, Raylib.GetScreenWidth() - (debugText.Length * (fontSize / 2)), 10, fontSize, Color.White);
    }

    public void Update(UpdateState updateState)
    {
    }
}
