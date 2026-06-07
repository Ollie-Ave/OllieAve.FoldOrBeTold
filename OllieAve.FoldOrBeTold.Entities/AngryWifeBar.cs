using System.Numerics;
using OllieAve.FoldOrBeTold.Entities.Helpers;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities;

public class AngryWifeBar : EntityBase, IEntity, IUiRenderable
{
    private const string barTextureName = "FlowerBar.png";
    private const string wifeTextureName = "HappyWife.png";

    private const int barXOffset = 30;

    private Vector2 position;
    private Texture2D barTexture;
    private Texture2D wifeTexture;

    private float percentTimeRemaining = 100;

    public AngryWifeBar()
    {
        barTexture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath(barTextureName));
        wifeTexture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath(wifeTextureName));

        position = new(10, Raylib.GetScreenHeight() - (barTexture.Height * 2) - 10);
    }

    public void Render()
    {
        float fullWidth = (barTexture.Width - 18 - 35 - 20) * 2;

        Rectangle bgRect = new(position.X + 35 + barXOffset, position.Y + 35, fullWidth, barTexture.Height - 7);
        Rectangle fillRect = new(position.X + 35 + barXOffset, position.Y + 35, fullWidth, barTexture.Height - 7);

        Raylib.DrawRectangleRounded(bgRect, 40, 0, Color.White);

        Rectangle scissor = new(
            fillRect.X,
            fillRect.Y,
            fullWidth * (percentTimeRemaining / 100f),
            fillRect.Height
        );

        Raylib.BeginScissorMode(
            (int)scissor.X,
            (int)scissor.Y,
            (int)scissor.Width,
            (int)scissor.Height
        );

        Raylib.DrawRectangleRounded(fillRect, 40, 0, Color.Red);

        Raylib.EndScissorMode();

        Raylib.DrawTextureEx(barTexture, position + new Vector2(barXOffset, 0), 0, 2, Color.White);
        Raylib.DrawTextureEx(wifeTexture, position, 0, 2, Color.White);
    }

    public void Update(UpdateState updateState)
    {
        position = new(10, Raylib.GetScreenHeight() - (barTexture.Height * 2) - 10);

        percentTimeRemaining -= 5 * Raylib.GetFrameTime();
    }
}
