using System.Numerics;
using OllieAve.FoldOrBeTold.Entities.Helpers;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities;

public class Player : EntityBase, IEntity, IRenderable
{
    private const string textureName = "Player.png";

    private const float acceleration = 30;
    private const float movementSpeed = 25;
    private const float maxVelocity = 5;
    private const float drag = 25;
    private const float reach = 50;

    private static Texture2D texture;
    private Vector2 velocity;
    private Vector2 position;

    private IEntity? itemHeldByPlayer;

    public int RenderingOrder => 1;

    public Player(Vector2 initialPos)
    {
        texture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath(textureName));
        position = initialPos;
    }


    public void Render()
    {
        Raylib.DrawTexture(texture, (int)position.X, (int)position.Y, Color.White);
    }

    public void Update(UpdateState state)
    {
        float frameTime = Raylib.GetFrameTime();

        velocity = ApplyDrag(frameTime);
        velocity = TakeInputControls(frameTime);

        position = UpdatePlayerPosition(frameTime);
    }

    public Vector2 GetPosition()
    {
        return position;
    }

    public bool CanReachItem(Vector2 itemPosition)
    {
        return Vector2.Distance(itemPosition, position) <= reach;
    }

    public Vector2 GetSize()
    {
        return new(texture.Width, texture.Height);
    }

    public IEntity? GetHeldItem()
    {
        return itemHeldByPlayer;
    }

    public void HoldItem(IEntity entity)
    {
        itemHeldByPlayer = entity;
    }

    public void DropItem()
    {
        itemHeldByPlayer = null;
    }

    private Vector2 UpdatePlayerPosition(float frameTime)
    {
        Vector2 position = new(this.position.X, this.position.Y);

        position.X += velocity.X * movementSpeed * frameTime;
        position.Y += velocity.Y * movementSpeed * frameTime;

        return position;
    }

    private Vector2 ApplyDrag(float frameTime)
    {
        Vector2 velocity = new(this.velocity.X, this.velocity.Y);

        if (velocity.X > 0 && !Raylib.IsKeyDown(KeyboardKey.D))
        {
            velocity.X = Math.Clamp(velocity.X - (drag * frameTime), 0, maxVelocity);
        }
        else if (velocity.X < 0 && !Raylib.IsKeyDown(KeyboardKey.A))
        {
            velocity.X = Math.Clamp(velocity.X + (drag * frameTime), -maxVelocity, 0);
        }

        if (velocity.Y > 0 && !Raylib.IsKeyDown(KeyboardKey.S))
        {
            velocity.Y = Math.Clamp(velocity.Y - (drag * frameTime), 0, maxVelocity);
        }
        else if (velocity.Y < 0 && !Raylib.IsKeyDown(KeyboardKey.W))
        {
            velocity.Y = Math.Clamp(velocity.Y + (drag * frameTime), -maxVelocity, 0);
        }

        return velocity;
    }

    private Vector2 TakeInputControls(float frameTime)
    {
        Vector2 velocity = new(this.velocity.X, this.velocity.Y);

        if (Raylib.IsKeyDown(KeyboardKey.W))
        {
            velocity.Y = Math.Clamp(velocity.Y - (acceleration * frameTime), -maxVelocity, maxVelocity);
        }

        if (Raylib.IsKeyDown(KeyboardKey.A))
        {
            velocity.X = Math.Clamp(velocity.X - (acceleration * frameTime), -maxVelocity, maxVelocity);
        }

        if (Raylib.IsKeyDown(KeyboardKey.S))
        {
            velocity.Y = Math.Clamp(velocity.Y + (acceleration * frameTime), -maxVelocity, maxVelocity);
        }

        if (Raylib.IsKeyDown(KeyboardKey.D))
        {
            velocity.X = Math.Clamp(velocity.X + (acceleration * frameTime), -maxVelocity, maxVelocity);
        }

        return velocity;
    }
}
