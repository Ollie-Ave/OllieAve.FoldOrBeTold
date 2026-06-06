using OllieAve.FoldOrBeTold.Entities;
using OllieAve.FoldOrBeTold.Entities.Helpers;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.App;

internal static class Program
{
    [STAThread]
    public static void Main()
    {
        Raylib.InitWindow(1920, 1080, "Hello World");

        Player player = new(new(150, 150));
        Tree tree = new(new(100, 100));
        Tree tree2 = new(new(300, 300));
        LaundryItem item = new(new(200, 200));
        WashingBasket basket = new(new(100, 150));

        EntityManager entityManager = new([player, tree, tree2, item, basket]);

        CameraHandler camera = new(entityManager);
        entityManager.AddEntity(camera);


        while (!Raylib.WindowShouldClose())
        {
            UpdateEntities(entityManager);

            Render(entityManager);
        }

        Raylib.CloseWindow();
    }

    private static void UpdateEntities(EntityManager entityManager)
    {
        UpdateState state = new()
        {
            EntityManager = entityManager,
        };

        foreach (IEntity entity in entityManager.GetEntitites())
        {
            entity.Update(state);
        }
    }

    private static void Render(EntityManager entityManager)
    {
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Color.Gray);

        Raylib.DrawLine(Raylib.GetScreenWidth() / 2, 0, Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight(), Color.Green);
        Raylib.DrawLine(0, Raylib.GetScreenHeight() / 2, Raylib.GetScreenWidth(), Raylib.GetScreenHeight() / 2, Color.Green);

        Raylib.BeginMode2D(entityManager.GetCamera().Camera);

        foreach (IRenderable renderable in entityManager.GetRenderables())
        {
            renderable.Render();
        }


        Raylib.EndMode2D();

        Raylib.EndDrawing();
    }
}
