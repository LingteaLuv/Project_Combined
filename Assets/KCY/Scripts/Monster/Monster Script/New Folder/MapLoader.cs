using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapLoader 
{
    public struct Map
    {
        public int Xsize;
        public int Ysize;
        public int Zsize;
        public int[,,] MapData;

        public Map(int x, int y, int z, int[,,] data)
        {
            Xsize = x;
            Ysize = y;
            Zsize = z;
            MapData = data;
        }
    }


    public static Map ReadMap(int map_index)
    {
        string path = $"Assets/Maps/Map_{map_index}.txt";
        if (!File.Exists(path))
        {
            Debug.LogError($"맵 파일 못찾음 {path}");   // 해당 경우 파일 이름확인 필요 ////
            return default;
        }

        string[] lines = File.ReadAllLines(path);

        string[] sizeParts = lines[0].Split();
        int Xsize = int.Parse(sizeParts[0]);
        int Ysize = int.Parse(sizeParts[1]);
        int Zsize = int.Parse(sizeParts[2]);

        int[,,] MapData = new int[Xsize, Ysize, Zsize];

        int lineIndex = 1;
        for (int z = 0; z < Zsize; z++)
        {
            for (int y = 0; y < Ysize; y++)
            {
                string[] row = lines[lineIndex++].Split();
                for (int x = 0; x < Xsize; x++)
                {
                    MapData[x, y, z] = int.Parse(row[x]);
                }
            }
        }
        return new Map(Xsize, Ysize, Zsize, MapData);
    }
}
