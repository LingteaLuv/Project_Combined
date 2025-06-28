using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDictionary : ScriptableObject
{
    [Header("Drag&Drop")] 
    [SerializeField] private TextAsset _enemyLoot;
    [SerializeField] private TextAsset _enemyLootGridChance;

    private Dictionary<string, List<int>> _lootTable;
    private Dictionary<string, List<int>> _lootGridTable;
    
    private void Awake()
    {
        Init();
        TableInit(_enemyLoot,_lootTable);
        TableInit(_enemyLootGridChance,_lootGridTable);
    }

    private void TableInit(TextAsset asset, Dictionary<string, List<int>> table)
    {
        if (asset == null) return;
        
        string[] lines = asset.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            // 문장의 앞,뒤 공백 제거
            string line = lines[i].Trim();

            // 공백을 제거했을 때 아무 것도 없는 경우 스킵
            if (string.IsNullOrEmpty(line)) continue;

            // 문장을 ,(쉼표)로 구분
            string[] parts = line.Split(',');

            string key = parts[0];
            
            List<int> values = new List<int>();
            
            for (int j = 1; j < parts.Length; j++)
            {
                values.Add(int.Parse(parts[i]));
            }
            table.Add(key,values);
        }
    }

    private void RandomItemSetting()
    {
        
    }
    
    private void Init()
    {
        _lootTable = new Dictionary<string, List<int>>();
        _lootGridTable = new Dictionary<string, List<int>>();
    }
}
