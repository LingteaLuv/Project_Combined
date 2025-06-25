using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarNode
{
    public bool Walkable;
    public int F => G + H;
    public int G;
    public int H;
    public AstarNode Parent;  // 이전 노드 

    public int X;
    public int Y;
    public int Z;

    public Transform Tile_transform; // 이거

    public AstarNode(bool walkable, int x, int y, int z, Transform tile)
    {
        Walkable = walkable;
        G = int.MaxValue; // 방문x 비용은 최대
        H = 0;
        X = x;
        Y = y;
        Z = z;
        Tile_transform = tile;
        Parent = null;
    }

    public Vector3Int Position => new Vector3Int(X, Y, Z);

}
