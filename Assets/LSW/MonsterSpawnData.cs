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
    
    public Dictionary<int,int[]> _dictionary;

    public int[] _enemyIds;


    public void SetList()
    {
        if (_dictionary.Count != 0) return;
        _dictionary = new Dictionary<int, int[]>();
        _dictionary.Add(Enemy1ID, new int[2] { Day1, Weight1 });
        _dictionary.Add(Enemy2ID, new int[2] { Day2, Weight1 });
        _dictionary.Add(Enemy3ID, new int[2] { Day3, Weight1 });
        _dictionary.Add(Enemy4ID, new int[2] { Day4, Weight1 });
        _dictionary.Add(Enemy5ID, new int[2] { Day5, Weight1 });
        _dictionary.Add(Enemy6ID, new int[2] { Day6, Weight1 });
        _dictionary.Add(Enemy7ID, new int[2] { Day7, Weight1 });

        if (_enemyIds.Length != 0) return;
        _enemyIds = new int[7] { Enemy1ID, Enemy2ID, Enemy3ID, Enemy4ID, Enemy5ID, Enemy6ID, Enemy7ID };
    }
}
