using System.Numerics;
using OllieAve.FoldOrBeTold.Entities.Helpers;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities;

public class AngryWifeBar : EntityBase, IEntity, IUiRenderable
{
    private const string barTextureName = "FlowerBar.png";
    private const string happyWifeTextureName = "HappyWife.png";
    private const string angryWifeTextureName = "AngryWife.png";
    private const string neutralWifeTextureName = "NeutralWife.png";
    private const string sadWifeTextureName = "SadWife.png";
    private const string veryHappyWifeTextureName = "VeryHappyWife-sheet.png";
    private const int wifeFaceFrameCount = 2; // inclusive of the first frame (0)
    private const int wifeFaceFrameHeight = 62;
    private const int wifeFaceFrameWidth = 60;
    private const float uiScaling = 2;
    private const float secondsBetweenWifeFaceFrames = 1;

    private const int barXOffset = 30;

    private Vector2 position;
    private Texture2D barTexture;
    private Texture2D happyWifeTexture;
    private Texture2D angryWifeTexture;
    private Texture2D neutralWifeTexture;
    private Texture2D sadWifeTexture;
    private Texture2D veryHappyWifeTexture;

    private int currentWifeFaceFrame;
    private float timeSinceLastWifeFaceFrameChange;

    public AngryWifeBar()
    {
        barTexture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath(barTextureName));

        happyWifeTexture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath(happyWifeTextureName));
        angryWifeTexture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath(angryWifeTextureName));
        neutralWifeTexture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath(neutralWifeTextureName));
        sadWifeTexture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath(sadWifeTextureName));
        veryHappyWifeTexture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath(veryHappyWifeTextureName));

        position = new(10, Raylib.GetScreenHeight() - (barTexture.Height * 2) - 10);
    }

    public void Render(RenderState state)
    {
        float fullWidth = (barTexture.Width - 18 - 35 - 20) * uiScaling;

        Rectangle bgRect = new(position.X + 35 + barXOffset, position.Y + 35, fullWidth, barTexture.Height - 7);
        Rectangle fillRect = new(position.X + 35 + barXOffset, position.Y + 35, fullWidth, barTexture.Height - 7);

        Raylib.DrawRectangleRounded(bgRect, 40, 0, Color.White);

        float percentLeft = state.EntityManager
            .GetStateManager()
            .GetPercentageOfTimeRemaining();

        Rectangle scissor = new(
            fillRect.X,
            fillRect.Y,
            fullWidth * (percentLeft / 100f),
            fillRect.Height
        );

        Raylib.BeginScissorMode(
            (int)scissor.X,
            (int)scissor.Y,
            (int)scissor.Width,
            (int)scissor.Height
        );

        Raylib.DrawRectangleRounded(fillRect, 40, 0, Color.Pink);

        Raylib.EndScissorMode();

        Raylib.DrawTextureEx(barTexture, position + new Vector2(barXOffset, 0), 0, uiScaling, Color.White);
        DrawWifeFace(percentLeft);
    }

    private void DrawWifeFace(float percentLeft)
    {
        Rectangle frame = new()
        {
            Height = wifeFaceFrameHeight,
            Width = wifeFaceFrameWidth,
            X = wifeFaceFrameWidth * currentWifeFaceFrame,
            Y = 0,
        };

        Rectangle positionRec = new()
        {
            Height = wifeFaceFrameHeight * uiScaling,
            Width = wifeFaceFrameWidth * uiScaling,
            X = position.X,
            Y = position.Y,
        };

        Raylib.DrawTexturePro(GetWifeFaceTextureToUse(percentLeft), frame, positionRec, Vector2.Zero, 0, Color.White);


        timeSinceLastWifeFaceFrameChange += Raylib.GetFrameTime();

        if (timeSinceLastWifeFaceFrameChange > secondsBetweenWifeFaceFrames)
        {
            timeSinceLastWifeFaceFrameChange = 0;
            currentWifeFaceFrame++;
        }

        if (currentWifeFaceFrame > wifeFaceFrameCount)
        {
            currentWifeFaceFrame = 0;
        }
    }

    private Texture2D GetWifeFaceTextureToUse(float percentLeft)
    {
        if (percentLeft <= 20)
        {
            return angryWifeTexture;
        }
        else if (percentLeft <= 40)
        {
            return sadWifeTexture;
        }
        else if (percentLeft <= 60)
        {
            return neutralWifeTexture;
        }
        else if (percentLeft <= 80)
        {
            return happyWifeTexture;
        }
        else
        {
            return veryHappyWifeTexture;
        }
    }

    public void Update(UpdateState updateState)
    {
        position = new(10, Raylib.GetScreenHeight() - (barTexture.Height * 2) - 10);
    }
}
