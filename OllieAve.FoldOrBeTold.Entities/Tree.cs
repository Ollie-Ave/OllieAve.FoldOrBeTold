using System.Numerics;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities;

public class Tree : EntityBase, IEntity, IRenderable
{
    private Vector2 position;

    public Tree(Vector2 position)
    {
        this.position = position;
    }

    public int RenderingOrder => 3;

    public void Render()
    {
        Raylib.DrawRectangle((int)position.X, (int)position.Y, 20, 40, Color.Brown);
        Raylib.DrawRectangle((int)position.X - 20, (int)position.Y - 20, 60, 20, Color.Green);
        Raylib.DrawRectangle((int)position.X - 10, (int)position.Y - 40, 40, 20, Color.Green);
    }

    public void Update(UpdateState state)
    {
    }
}
