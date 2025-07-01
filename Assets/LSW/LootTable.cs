using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLootTable : MonoBehaviour
{
    [SerializeField] private LootDictionary _lootDic;
    [SerializeField] private ItemDictionary _itemDic;
    // [SerializeField] private MonsterDictionary _monsterDic; 
    [SerializeField] private string _monsterID;
    
    private string _id;
    private string _gridId;
    
    public List<int> _itemId;
    public List<int> _itemStack;
    public List<int> _itemWeight;

    private int _itemCount;
    
    public List<ItemBase> _resultItems;
    public List<int> _resultItemAmount;

    private void Start()
    {
        //todo 몬스터 도감이 완성되면 참조 걸고 주석 해제
        //_id = _monsterDic.monsterInfo[_monsterID].LootID;
        //_gridId = _monsterDic.monsterInfo[_monsterID].LootGridChanceID;
        GenerateItem();
    }
    
    public void GenerateItem()
    {
        _lootDic.GenerateDic();
        _itemDic.GenerateDic();
        Init();
        int gridNum = SetGridAmount();
        SetItem(gridNum);
    }
    
    private void Init()
    {
        _itemId = new List<int>();
        _itemStack = new List<int>();
        _itemWeight = new List<int>();

        for (int i = 0; i < _lootDic.LootTable[_id].Count; i+=3)
        {
            _itemId.Add(_lootDic.LootTable[_id][i]);
            _itemStack.Add(_lootDic.LootTable[_id][i+1]);
            _itemWeight.Add(_lootDic.LootTable[_id][i+2]);
        }

        _itemCount = _itemId.Count;
    }
    
    private int SetGridAmount()
    {
        int volume = _lootDic.LootGridTable[_gridId].Count;
        int[] weightSums = new int[volume];
        for (int i = 0; i < volume; i++)
        {
            for (int j = 0; j < i + 1; j++)
            {
                weightSums[i] += _lootDic.LootGridTable[_gridId][j];
            }
        }
        int rndNum = Random.Range(0, weightSums[volume - 1]);
        
        for (int i = 0; i < volume; i++)
        {
            if (rndNum < weightSums[i]) return i < _itemCount ? i : _itemCount;
        }
        return -1;
    }

    private void SetItem(int volume)
    {
        _resultItems = new List<ItemBase>();
        _resultItemAmount = new List<int>();
        int remaining = volume;
        
        while (remaining > 0)
        {
            if (_itemCount == 0) break;
            
            int[] weightSums = new int[_itemCount];
            
            for (int i = 0; i < _itemCount; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    weightSums[i] += _itemWeight[j];
                }
            }
        
            int rndNum = Random.Range(0, weightSums[_itemCount - 1]);
            
            for (int i = 0; i < _itemCount; i++)
            {
                if (rndNum < weightSums[i])
                {
                    _resultItems.Add(_itemDic.ItemDic[_itemId[i]]);
                    _resultItemAmount.Add(_itemStack[i]);
                    _itemId.Remove(_itemId[i]);
                    _itemStack.Remove(_itemStack[i]);
                    _itemWeight.Remove(_itemWeight[i]);
                    remaining--;
                    _itemCount--;
                    break;
                }
            }
        }
    }
}

