using System.Numerics;
using OllieAve.FoldOrBeTold.Entities.Helpers;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities;

public class CameraHandler : EntityBase, IEntity
{
    public float CameraZoom { get; set; } = 3.0f;

    private const float DeadZone = 50f;
    private const float FollowStrength = 8f;   // responsiveness
    private const float MaxOffsetSpeed = 600f; // clamp safety

    private Camera2D camera;

    public Camera2D Camera
    {
        get => camera;
        set => camera = value;
    }

    public CameraHandler(EntityManager entityManager)
    {
        Vector2 playerPos = entityManager.GetPlayer().GetPosition();
        Vector2 playerSize = entityManager.GetPlayer().GetSize();

        Vector2 playerCenter = playerPos + (playerSize / 2f);

        Vector2 offset = new(
            Raylib.GetScreenWidth() / 2,
            Raylib.GetScreenHeight() / 2
        );

        camera = new Camera2D
        {
            Offset = offset,
            Target = playerCenter,
            Rotation = 0f,
            Zoom = CameraZoom
        };
    }

    public void Update(UpdateState updateState)
    {
        HandleCameraPosition(updateState);

        HandleZoomControl();
    }

    private void HandleZoomControl()
    {
        float mouseWheelMove = Raylib.GetMouseWheelMove();

        if (mouseWheelMove != 0)
        {
            CameraZoom += mouseWheelMove / 3;
            Console.WriteLine("Scrolled Up");
        }

        camera.Zoom = CameraZoom;
    }

    private void HandleCameraPosition(UpdateState updateState)
    {
        Vector2 playerPos = updateState.EntityManager.GetPlayer().GetPosition();
        Vector2 playerSize = updateState.EntityManager.GetPlayer().GetSize();

        Vector2 playerCenter = playerPos - (playerSize / 2f);
        //Vector2 playerCenter = playerPos * 2;

        Vector2 camCenter = camera.Target;

        Vector2 delta = playerCenter - camCenter;

        Vector2 correction = Vector2.Zero;

        // Deadzone on X
        if (MathF.Abs(delta.X) > DeadZone)
        {
            correction.X = delta.X - MathF.Sign(delta.X) * DeadZone;
        }

        // Deadzone on Y
        if (MathF.Abs(delta.Y) > DeadZone)
        {
            correction.Y = delta.Y - MathF.Sign(delta.Y) * DeadZone;
        }

        float dt = Raylib.GetFrameTime();

        // Smooth follow only when outside deadzone
        camera.Target += Vector2.Clamp(
            correction * FollowStrength * dt,
            new Vector2(-MaxOffsetSpeed),
            new Vector2(MaxOffsetSpeed)
        );
    }
}
