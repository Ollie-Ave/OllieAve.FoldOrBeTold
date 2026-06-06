using System.Numerics;
using OllieAve.FoldOrBeTold.Entities.Helpers;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities;

public class WashingBasket : IEntity, IRenderable
{
    private const string textureName = "WashingBasket.png";
    private static Texture2D texture;
    private Vector2 position;

    public WashingBasket(Vector2 position)
    {
        this.position = position;
        texture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath(textureName));
    }

    public int RenderingOrder => 2;

    public void Render()
    {
        Raylib.DrawTexture(texture, (int)position.X, (int)position.Y, Color.White);
    }

    public void Update(UpdateState updateState)
    {
    }
}
