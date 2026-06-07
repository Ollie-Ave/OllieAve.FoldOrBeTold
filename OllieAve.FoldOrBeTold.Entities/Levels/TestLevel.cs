using System.Numerics;
using OllieAve.FoldOrBeTold.Entities.Helpers;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;
using TiledCS;

namespace OllieAve.FoldOrBeTold.Entities.Levels;

public class TestLevel : EntityBase, IEntity, IRenderable
{
    private readonly Texture2D tilesetTexture;
    private readonly int firstGid;

    private readonly TiledMap map;
    private readonly TiledTileset tileset;

    public TestLevel()
    {
        map = new TiledMap(FileSystemHelper.GetLevelFilePath("TestMap.tmx"));
        tileset = new TiledTileset(FileSystemHelper.GetLevelFilePath("HouseTilemap.tsx"));

        firstGid = tileset.Tiles.First(x => x.id != 0).id;

        tilesetTexture = Raylib.LoadTexture(FileSystemHelper.GetAssetPath("HouseTilemap.png"));
    }

    public int RenderingOrder => 10;

    public void Render()
    {
        foreach (TiledLayer layer in map.Layers)
        {
            DrawTileLayer(layer);
        }
    }

    public void Update(UpdateState updateState)
    {
    }

    private void DrawTileLayer(TiledLayer layer)
    {
        int[] tiles = layer.data;

        int offsetX = 0;
        int offsetY = 0;

        for (int i = 0; i < tiles.Length; i++)
        {
            int gid = tiles[i];

            if (gid == 0)
                continue;

            int x = (i % layer.width) + offsetX;
            int y = (i / layer.width) + offsetY;

            int localId = gid - firstGid;

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
        return;
    }
}
