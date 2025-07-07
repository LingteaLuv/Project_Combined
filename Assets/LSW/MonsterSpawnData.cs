using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "MonsterSpawnData")]
public class MonsterSpawnData : ScriptableObject
{
    public int SpawnID;
    public int Day1;
    public int Enemy1ID;
    public int Weight1;
    public int Day2;
    public int Enemy2ID;
    public int Weight2;
    public int Day3;
    public int Enemy3ID;
    public int Weight3;
    public int Day4;
    public int Enemy4ID;
    public int Weight4;
    public int Day5;
    public int Enemy5ID;
    public int Weight5;
    public int Day6;
    public int Enemy6ID;
    public int Weight6;
    public int Day7;
    public int Enemy7ID;
    public int Weight7;
    
    public int[][] _dictionary;

    public int[] _enemyIds;
    

    public void SetList()
    {
        if (_dictionary != null) return;
        _dictionary = new int[7][];
        _dictionary[0] = new int[3] { Day1, Enemy1ID, Weight1 };
        _dictionary[1] = new int[3] { Day2, Enemy2ID, Weight2 };
        _dictionary[2] = new int[3] { Day3, Enemy3ID, Weight3 };
        _dictionary[3] = new int[3] { Day4, Enemy4ID, Weight4 };
        _dictionary[4] = new int[3] { Day5, Enemy5ID, Weight5 };
        _dictionary[5] = new int[3] { Day6, Enemy6ID, Weight6 };
        _dictionary[6] = new int[3] { Day7, Enemy7ID, Weight7 };

        if (_enemyIds.Length != 0) return;
        _enemyIds = new int[7] { Enemy1ID, Enemy2ID, Enemy3ID, Enemy4ID, Enemy5ID, Enemy6ID, Enemy7ID };
    }
}
