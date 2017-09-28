using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : Singleton<MapData>
{
    private const int maxWidth = 50;
    private const int maxHeight = 50;
    private const float tileSize = 1f;

    private TileController[,] tileMap;

    private MiniPool<TileController> tilePool;
    private Transform tileRoot;
    private Transform hideTileRoot;

    protected override void Init()
    {
        tileRoot = Util.FindObject(transform, "Tiles").transform;
        hideTileRoot = Util.FindObject(transform, "Hide").transform;
        tilePool = MiniPool<TileController>.MakeMiniPool(hideTileRoot, Resources.Load("Tile") as GameObject);
    }

    public void CreateTiles()
    {
        tileMap = new TileController[maxWidth, maxHeight];
        
        for (int y = 0; y < maxHeight; y++)
        {
            for (int x = 0; x < maxWidth; x++)
            {
                TileController tile = tilePool.GetItem(tileRoot, GetPosition(x, y));
                tile.Init(x, y);
                tileMap[x, y] = tile;
            }
        }
    }

    private Vector3 GetPosition(int width, int height)
    {
        return new Vector3((width - maxWidth * 0.5f) * tileSize, 0f, (height - maxHeight * 0.5f) * tileSize);
    }
}
