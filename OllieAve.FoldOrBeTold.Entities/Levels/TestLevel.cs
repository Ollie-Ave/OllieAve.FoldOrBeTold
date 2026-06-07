using System.Numerics;
using DotTiled;
using DotTiled.Serialization;
using OllieAve.FoldOrBeTold.Entities.Helpers;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities.Levels;

public class TestLevel : EntityBase, IEntity, IRenderable
{
    private readonly Map map;
    private readonly Texture2D tilesetTexture;
    private readonly Tileset tileset;
    private readonly uint firstGid;

    public TestLevel()
    {
        Loader loader = Loader.Default();

        map = loader.LoadMap(
            FileSystemHelper.GetLevelFilePath("TestMap.tmx"));

        var tilesetReference = map.Tilesets[0];

        tileset = tilesetReference
            ?? throw new InvalidOperationException("Tileset not loaded.");

        firstGid = tilesetReference.FirstGID.Value;

        tilesetTexture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath("HouseTilemap.png"));
    }

    public int RenderingOrder => 10;

    public void Render()
    {
        foreach (BaseLayer? baseLayer in map.Layers)
        {
            if (baseLayer is not TileLayer layer)
            {
                continue;
            }

            DrawTileLayer(layer);
        }
    }

    public void Update(UpdateState updateState)
    {
    }

    private void DrawTileLayer(TileLayer layer)
    {
        if (layer.Data.Value is null)
            return;

        var data = layer.Data.Value;

        uint[] tiles;

        // flat map
        if (data.GlobalTileIDs.HasValue)
        {
            tiles = data.GlobalTileIDs.Value;
            DrawTiles(tiles, map.Width);
            return;
        }

        // infinite map fallback
        if (data.Chunks.HasValue)
        {
            foreach (var chunk in data.Chunks.Value)
            {
                var chunkTiles = chunk.GlobalTileIDs;

                DrawTiles(chunkTiles, chunk.Width, chunk.X, chunk.Y);
            }
        }
    }

    private void DrawTiles(uint[] tiles, int width, int offsetX = 0, int offsetY = 0)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            uint gid = tiles[i];

            if (gid == 0)
                continue;

            int x = (i % width) + offsetX;
            int y = (i / width) + offsetY;

            int localId = (int)(gid - firstGid);

            int srcX = localId % tileset.Columns * tileset.TileWidth;
            int srcY = localId / tileset.Columns * tileset.TileHeight;

            Rectangle src = new(
                srcX,
                srcY,
                tileset.TileWidth,
                tileset.TileHeight);

            Rectangle dst = new(
                x * map.TileWidth,
                y * map.TileHeight,
                map.TileWidth,
                map.TileHeight);

            Raylib.DrawTexturePro(
                tilesetTexture,
                src,
                dst,
                Vector2.Zero,
                0f,
                Color.White);
        }
    }
}
