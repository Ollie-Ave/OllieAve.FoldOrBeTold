using System.Numerics;
using OllieAve.FoldOrBeTold.Entities.Constants;
using OllieAve.FoldOrBeTold.Entities.Helpers;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities;

public class WashingBasket : EntityBase, IEntity, IRenderable
{
    private const float shakingAnimationDuration = 0.1f;
    private const float growAnimationDuration = 0.2f;
    private const string textureName = "WashingBasket.png";
    private static Texture2D texture;
    private Vector2 position;
    private Shader shader;

    private bool hovered;

    private bool IsShaking => timeLeftShakingSeconds > 0;
    private bool IsGrowing => timeLeftGrowingSeconds > 0;
    private float timeLeftShakingSeconds;
    private float timeLeftGrowingSeconds;

    public WashingBasket(Vector2 position)
    {
        texture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath(textureName));

        shader = Raylib.LoadShader(null, FileSystemHelper.GetShaderPath(ShaderNames.WhiteOutlineShaderName));
        SetupShader();

        this.position = position;
    }

    public int RenderingOrder => 2;

    public void Render()
    {
        if (hovered)
            Raylib.BeginShaderMode(shader);

        int xOffset = 0;
        int yOffset = 0;

        if (IsShaking)
        {
            Random r = new();
            xOffset = r.Next(-2, 2);
            yOffset = r.Next(-2, 2);
        }

        float scale = 1;

        if (IsGrowing)
        {
            scale += timeLeftGrowingSeconds * 0.3f;
        }

        Rectangle source = new(
            0,
            0,
            texture.Width,
            texture.Height);

        float scaledWidth = texture.Width * scale;
        float scaledHeight = texture.Height * scale;

        Rectangle dest = new(
            position.X - (scaledWidth - texture.Width) / 2f + xOffset,
            position.Y - (scaledHeight - texture.Height) / 2f + yOffset,
            scaledWidth,
            scaledHeight);

        Raylib.DrawTexturePro(
            texture,
            source,
            dest,
            Vector2.Zero,
            0,
            Color.White);

        if (hovered)
            Raylib.EndShaderMode();
    }

    public void Update(UpdateState state)
    {
        HandleHoverShaderApplication(state);
        HandlePlace(state);
        HandleShakingTimer();
        HandleGrowingTimer();
    }

    private void HandleGrowingTimer()
    {
        if (!IsGrowing) return;

        timeLeftGrowingSeconds = Math.Clamp(
                timeLeftGrowingSeconds - Raylib.GetFrameTime(),
                0,
                timeLeftGrowingSeconds);
    }

    private void HandleShakingTimer()
    {
        if (!IsShaking) return;

        timeLeftShakingSeconds = Math.Clamp(
                timeLeftShakingSeconds - Raylib.GetFrameTime(),
                0,
                timeLeftShakingSeconds);
    }

    private void HandlePlace(UpdateState state)
    {
        Player player = state.EntityManager.GetPlayer();

        if (!hovered || !player.CanReachItem(position) || !Raylib.IsMouseButtonReleased(MouseButton.Left)) return;

        IEntity? heldItem = player.GetHeldItem();

        if (heldItem is null)
        {
            timeLeftShakingSeconds = shakingAnimationDuration;
        }
        else
        {
            state.EntityManager.DespawnEntity(heldItem.EntityId);
            player.DropItem();
            timeLeftGrowingSeconds = growAnimationDuration;
        }
    }


    private void HandleHoverShaderApplication(UpdateState state)
    {
        Vector2 mousePos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), state.EntityManager.GetCamera().Camera);
        Rectangle mouseRec = new(mousePos.X, mousePos.Y, 1, 1);

        Rectangle itemRec = new(position.X, position.Y, texture.Width, texture.Height);

        hovered = state.EntityManager.GetPlayer().CanReachItem(position)
            && Raylib.CheckCollisionRecs(itemRec, mouseRec);
    }

    private void SetupShader()
    {
        Raylib.SetTextureWrap(texture, TextureWrap.Clamp);

        float outlineSize = 1.0f;
        float[] outlineColor = [1.0f, 1.0f, 1.0f, 1.0f];
        float[] textureSize = [texture.Width, texture.Height];

        int outlineSizeLoc = Raylib.GetShaderLocation(shader, "outlineSize");
        int outlineColorLoc = Raylib.GetShaderLocation(shader, "outlineColor");
        int textureSizeLoc = Raylib.GetShaderLocation(shader, "textureSize");

        Raylib.SetShaderValue(shader, outlineSizeLoc, outlineSize, ShaderUniformDataType.Float);
        Raylib.SetShaderValue(shader, outlineColorLoc, outlineColor, ShaderUniformDataType.Vec4);
        Raylib.SetShaderValue(shader, textureSizeLoc, textureSize, ShaderUniformDataType.Vec2);
    }
}

