using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLootTable : MonoBehaviour
{
    [SerializeField] private LootDictionary _lootDic;
    [SerializeField] private string _id;
    [SerializeField] private ItemDictionary _itemDic;
    
    private List<int> _itemId;
    private List<int> _itemStack;
    private List<int> _itemWeight;

    private int _itemCount;

    // todo : 생성한 아이템, 수량 Lootable class에 연동하는 작업
    public List<ItemBase> _resultItems;
    public List<int> _resultItemAmount;

    private void Start()
    {
        GenerateItem();
    }
    
    public void GenerateItem()
    {
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
        int volume = _lootDic.LootGridTable[_id].Count;
        int[] weightSums = new int[volume];
        for (int i = 0; i < volume; i++)
        {
            for (int j = 0; j < i + 1; j++)
            {
                weightSums[i] += _lootDic.LootGridTable[_id][j];
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
        int remaining = _itemCount;
        
        while (remaining > 0)
        {
            if (_itemId.Count == 0) break;
            
            int[] weightSums = new int[remaining];
            
            for (int i = 0; i < remaining; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    weightSums[i] += _itemWeight[j];
                }
            }
        
            int rndNum = Random.Range(0, weightSums[remaining - 1]);
            
            for (int i = 0; i < remaining; i++)
            {
                if (rndNum < weightSums[i])
                {
                    _resultItems.Add(_itemDic.ItemDic[_itemId[i]]);
                    _resultItemAmount.Add(_itemStack[i]);
                    _itemId.Remove(_itemId[i]);
                    _itemStack.Remove(_itemStack[i]);
                    _itemWeight.Remove(_itemWeight[i]);
                    remaining--;
                    break;
                }
            }
        }
    }
}

