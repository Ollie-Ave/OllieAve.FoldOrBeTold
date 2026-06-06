using System.Numerics;
using OllieAve.FoldOrBeTold.Entities.Helpers;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities;

public class LaundryItem : IEntity, IRenderable
{
    private const string textureName = "Shirt.png";
    private const string outlineShaderName = "WhiteOutline.fs";
    private readonly Vector2 pickedUpOffset = new(15, 15);
    private readonly Vector2 putDownOffset = new(20, 12);

    private static Texture2D texture;
    private Shader shader;

    private bool hovered;
    private bool pickedUp;

    private Vector2 position;

    public int RenderingOrder => 2;

    public LaundryItem(Vector2 position)
    {
        texture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath(textureName));

        shader = Raylib.LoadShader(null, FileSystemHelper.GetShaderPath(outlineShaderName));
        SetupShader();

        this.position = position;
    }

    public void Render()
    {
        if (pickedUp)
        {
            Raylib.DrawTextureEx(texture, position, 15, 0.7f, Color.White);

            return;
        }

        if (hovered)
            Raylib.BeginShaderMode(shader);

        Raylib.DrawTexture(texture, (int)position.X, (int)position.Y, Color.White);

        if (hovered)
            Raylib.EndShaderMode();
    }

    public void Update(UpdateState state)
    {
        HandleHoverShaderApplication(state);

        HandlePickupInput(state);

        HandlePickedUp(state);
    }

    private void HandlePickupInput(UpdateState state)
    {
        if (hovered && Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            pickedUp = true;
            hovered = false;

            Player player = state.EntityManager.GetPlayer();
            position = player.GetPosition() + pickedUpOffset;
        }

        if (pickedUp && Raylib.IsMouseButtonDown(MouseButton.Right))
        {
            pickedUp = false;

            Player player = state.EntityManager.GetPlayer();
            position = player.GetPosition() + putDownOffset;
        }
    }

    private void HandlePickedUp(UpdateState state)
    {
        if (!pickedUp) return;

        Player player = state.EntityManager.GetPlayer();

        position = player.GetPosition() + pickedUpOffset;
    }

    private void HandleHoverShaderApplication(UpdateState state)
    {
        if (pickedUp) return;

        Vector2 mousePos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), state.EntityManager.GetCamera().Camera);
        Rectangle mouseRec = new(mousePos.X, mousePos.Y, 1, 1);

        Rectangle itemRec = new(position.X, position.Y, texture.Width, texture.Height);

        hovered = Raylib.CheckCollisionRecs(itemRec, mouseRec);
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
