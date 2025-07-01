
using System.Collections.Generic;

using UnityEngine;


public class CraftingController : MonoBehaviour
{
    [SerializeField] InventoryModel _model;
    public Dictionary<int, int> CountByID { get; set; } //id를 통해 현재 가진 아이템의 개수를 가져오기 위함
    
    // ScriptableObject - New Item Dictionary 드래그해서 가져오기
    [SerializeField] private ItemDictionary _itemDictionary;

    private InventoryController _control;
    private InventoryRenderer _renderer;

    [SerializeField] private RecipeSetting _rs;

    private void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        _renderer = GetComponent<InventoryRenderer>();
        _control = GetComponent<InventoryController>();
        CountByID = new Dictionary<int, int>();
        
        _itemDictionary.GenerateDic();
        foreach (ItemBase i in _itemDictionary.ItemDic.Values)
        {
            CountByID.Add(i.ItemID, 0);
        }
    }

    private void Start()
    {
        LinkButton();
        UIBinder.Instance.GetInventory(CountByID);
    }
    
    // UI 각 버튼에 해당 레시피에 대한 Craft() 연동하는 메서드 
    private void LinkButton()
    {
        for (int i = 0; i < _itemDictionary.RecipeDic.Count; i++)
        {
            int index = i;
            UIBinder.Instance.GetCraftingUI().CreateBtn[index].onClick.AddListener
                (() => Craft(_itemDictionary.RecipeDic[_itemDictionary.RecipeKeys[index]]));
        }
    }
    

    public void UpdateCurrent()
    {
        UIBinder.Instance.GetInventory(CountByID);
    }
    public void Add(int id, int count)
    {
        CountByID[id] += count;
    }

    public bool AddItemByID(int id, int count, int dur)
    {
        if (_itemDictionary.ItemDic.TryGetValue(id, out ItemBase item))
        {
            return _control.AddItem(item, count, dur);
        }
        return false;
    }
    public bool RemoveItemByID(int id, int count)
    {
        if (_itemDictionary.ItemDic.TryGetValue(id, out ItemBase item))
        {
            return _control.RemoveItem(item, count);
        }
        return false;
    }

    private int GetMaxDur(int id)
    {
        ItemBase item = _itemDictionary.ItemDic[id];
        if (item.Type == ItemType.Melee)
        {
            return (item as MeleeItem).MaxDurability;
        }
        else if (item.Type == ItemType.Shield)
        {
            return (item as ShieldItem).MaxDurability;
        }
        else if (item.Type == ItemType.Special)
        {
            return (item as SpecialItem).MaxDurability;
        }
        else
        {
            return -1;
        }
    }
    
    private bool Craft(Recipe recipe)
    {
        if (recipe.MaterialItemId1 != 0) // 레시피 아이템이 설정됨
        {
            if (CountByID[recipe.MaterialItemId1] < recipe.MaterialItemQuantity1) 
            {
                Debug.Log("재료가 부족합니다");
                return false; //부족
            }
        }
        if (recipe.MaterialItemId2 != 0) 
        {
            if (CountByID[recipe.MaterialItemId2] < recipe.MaterialItemQuantity2) 
            {
                Debug.Log("재료가 부족합니다");
                return false; 
            }
        }
        if (recipe.MaterialItemId3 != 0) 
        {
            if (CountByID[recipe.MaterialItemId3] < recipe.MaterialItemQuantity3)
            {
                Debug.Log("재료가 부족합니다");
                return false; 
            }
        }
        if (recipe.MaterialItemId4 != 0) 
        {
            if (CountByID[recipe.MaterialItemId4] < recipe.MaterialItemQuantity4)
            {
                Debug.Log("재료가 부족합니다");
                return false; 
            }
        }
        RemoveItemByID(recipe.MaterialItemId1, recipe.MaterialItemQuantity1);
        RemoveItemByID(recipe.MaterialItemId2, recipe.MaterialItemQuantity2);
        RemoveItemByID(recipe.MaterialItemId3, recipe.MaterialItemQuantity3);
        RemoveItemByID(recipe.MaterialItemId4, recipe.MaterialItemQuantity4);

        AddItemByID(recipe.ResultItemId, recipe.ResultQuantity, GetMaxDur(recipe.ResultItemId));
        UpdateCurrent();
        StartCoroutine(_rs.DelayedUIUpdate());
        _renderer.RenderInventory();
        return true;
    }
}
