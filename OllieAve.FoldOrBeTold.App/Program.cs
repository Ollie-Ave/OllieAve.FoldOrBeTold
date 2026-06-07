using System.Numerics;
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
        Raylib.SetConfigFlags(ConfigFlags.UndecoratedWindow);
        Raylib.InitWindow(Raylib.GetMonitorWidth(0), Raylib.GetMonitorHeight(0), "game");

        Player player = new(new(150, 150));
        Tree tree = new(new(100, 100));
        Tree tree2 = new(new(300, 300));
        WashingBasket basket = new(new(100, 150));
        AngryWifeBar wifeBar = new();
        StateManager stateManager = new();

        EntityManager entityManager = new([player, tree, tree2, basket, stateManager, wifeBar]);

        for (int i = 0; i < 10; i++)
        {
            Random random = new();
            Vector2 newPos = new(random.Next(-100, 300), random.Next(-100, 300));
            LaundryItem item = new(newPos);

            entityManager.SpawnEntity(item);
        }

        CameraHandler camera = new(entityManager);
        entityManager.SpawnEntity(camera);


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

        foreach (IUiRenderable renderable in entityManager.GetUiRenderables())
        {
            RenderState state = new()
            {
                EntityManager = entityManager,
            };

            renderable.Render(state);
        }

        Raylib.EndDrawing();
    }
}
