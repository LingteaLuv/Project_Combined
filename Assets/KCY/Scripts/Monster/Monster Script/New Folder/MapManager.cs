using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject Tile;
    public int Map_index;

    public MapLoader.Map CurrentMap;
    public AstarNode[,,] map;


    public List<AstarNode> GetNeighbour(AstarNode node)
    {
        List<AstarNode> neighbours = new List<AstarNode>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dz = -1; dz <= 1; dz++)
                {
                    if (dx == 0 && dy == 0 && dz == 0)
                        continue;

                    int nx = node.X + dx;
                    int ny = node.Y + dy;
                    int nz = node.Z + dz;


                    if (nx >= 0 && nx < CurrentMap.Xsize &&
                        ny >= 0 && ny < CurrentMap.Ysize &&
                        nz >= 0 && nz < CurrentMap.Zsize &&
                        map[nx, ny, nz].Walkable)
                    {
                        neighbours.Add(map[nx, ny, nz]);
                    }

                }
            }
        }
        return neighbours;
    }


    private void Awake()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Start()
    {
        CurrentMap = MapLoader.ReadMap(Map_index);
        map = new AstarNode[CurrentMap.Xsize, CurrentMap.Ysize, CurrentMap.Zsize];

        for (int z = 0; z < CurrentMap.Zsize; z++)
        {
            for (int y = 0; y < CurrentMap.Ysize; y++)
            {
                for (int x = 0; x < CurrentMap.Xsize; x++)
                {
                    bool isWalkable = CurrentMap.MapData[x, y, z] == 1;

                    GameObject temp_tile = Instantiate(Tile, new Vector3(x,y,z), Quaternion.identity, transform);
                    map[x, y, z] = new AstarNode(isWalkable, x, y, z, temp_tile.transform);

                    if (!isWalkable)
                    {
                        temp_tile.GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 0, 0, 0.5f));
                    }
                }
                
            }
        }
    }
}
