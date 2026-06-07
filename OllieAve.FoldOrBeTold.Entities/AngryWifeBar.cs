using System.Numerics;
using OllieAve.FoldOrBeTold.Entities.Helpers;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities;

public class AngryWifeBar : EntityBase, IEntity, IUiRenderable
{
    private const string textureName = "FlowerBar.png";
    private const string shaderName = "PillMask.fs";

    private Vector2 position;
    private Texture2D texture;
    private Shader shader;
    private RenderTexture2D target;

    private float percentTimeRemaining = 100;

    public AngryWifeBar()
    {
        texture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath(textureName));
        shader = Raylib.LoadShader(null, FileSystemHelper.GetShaderPath(shaderName));
        SetupShader();

        target = Raylib.LoadRenderTexture(texture.Width, texture.Height);

        position = new(10, Raylib.GetScreenHeight() - (texture.Height * 2) - 10);
    }

    public void Render()
    {
        float fullWidth = (texture.Width - 18 - 35 - 20) * 2;

        Rectangle bgRect = new(position.X + 35, position.Y + 35, fullWidth, texture.Height - 7);
        Rectangle fillRect = new(position.X + 35, position.Y + 35, fullWidth, texture.Height - 7);

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

        Raylib.DrawTextureEx(texture, position, 0, 2, Color.White);
    }

    public void Update(UpdateState updateState)
    {
        position = new(10, Raylib.GetScreenHeight() - (texture.Height * 2) - 10);

        percentTimeRemaining -= 5 * Raylib.GetFrameTime();
    }

    private void SetupShader()
    {
    }
}
