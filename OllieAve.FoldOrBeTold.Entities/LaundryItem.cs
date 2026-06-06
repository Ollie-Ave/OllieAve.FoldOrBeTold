using System.Numerics;
using OllieAve.FoldOrBeTold.Entities.Constants;
using OllieAve.FoldOrBeTold.Entities.Helpers;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities;

public class LaundryItem : EntityBase, IEntity, IRenderable
{
    private static readonly List<string> textureNames = [
        "BlueBra.png",
        "PinkBra.png",
        "ShirtBlank.png",
        "ShirtButtons.png",
        "ShirtCat.png",
        "ShirtCherry.png",
        "ShirtCreeper.png",
        "ShirtDaisy.png",
        "ShirtHeart.png",
        "ShirtRainbow.png",
        "ShirtRose.png",
        "ShirtSmile.png",
        "ShirtStrawberry.png",
        "ShirtStripes.png",
        "SweaterBlank.png",
        "SweaterDots.png",
        "YellowBra.png",
    ];

    private readonly Vector2 pickedUpOffset = new(15, 15);
    private readonly Vector2 putDownOffset = new(20, 12);

    private Texture2D texture;
    private Shader shader;

    private bool hovered;
    private bool pickedUp;

    private Vector2 position;

    public int RenderingOrder => 2;

    public LaundryItem(Vector2 position)
    {
        Random random = new();
        string textureName = textureNames[random.Next(0, textureNames.Count)];
        texture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath(textureName));

        shader = Raylib.LoadShader(null, FileSystemHelper.GetShaderPath(ShaderNames.WhiteOutlineShaderName));
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
        Player player = state.EntityManager.GetPlayer();
        Vector2 playerPosition = player.GetPosition();

        if (hovered
            && Raylib.IsMouseButtonDown(MouseButton.Left)
            && player.CanReachItem(position))
        {
            pickedUp = true;
            hovered = false;

            player.HoldItem(this);

            position = playerPosition + pickedUpOffset;
        }

        if (pickedUp && Raylib.IsMouseButtonDown(MouseButton.Right))
        {
            pickedUp = false;

            player.DropItem();

            position = playerPosition + putDownOffset;
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
