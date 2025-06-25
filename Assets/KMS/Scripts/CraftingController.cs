using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;

public class CraftingController : MonoBehaviour
{
    [SerializeField] InventoryModel _model;

    public Dictionary<int, int> CountByID { get; set; } //id를 통해 현재 가진 아이템의 개수를 가져오기 위함
    public Dictionary<int, ItemBase> ItemIDs { get; set; } //id를 통해 itembase를 불러오기 위함

    public Dictionary<int, Recipe> RecipeIDs { get; set; }

    public RecipeSOList RecipeList { get; set; }

    public InventoryController Controller { get; }


    private void Awake()
    {
        CountByID = new Dictionary<int, int>();
        ItemIDs = new();
        RecipeIDs = new();
        Init();
    }
    private void Init()
    {
        foreach (ItemBase i in _model.ItemList.ItemList)
        {
            ItemIDs.Add(i.ItemID, i);
            CountByID.Add(i.ItemID, 0);
        }
        //foreach (Recipe i in RecipeList.RecipeList)
        //{
        //    RecipeIDs.Add(i.ItemId, i);
        //}
    }
    private bool IsplayerHave(int id)
    {
        return CountByID.ContainsKey(id);
    }

    public void Start()
    {
        //Add(6301, 5);
    }

    public void Add(int id, int count)
    {
        CountByID[id] += count;
    }

    public bool AddItemByID(int id, int count, int dur)
    {
        if (ItemIDs.TryGetValue(id, out ItemBase item))
        {
            return Controller.AddItem(item, count, dur);
        }
        return false;
    }
    public bool RemoveItemByID(int id, int count)
    {
        if (ItemIDs.TryGetValue(id, out ItemBase item))
        {
            return Controller.RemoveItem(item, count);
        }
        return false;
    }

    private int GetMaxDur(int id)
    {
        ItemBase item = ItemIDs[id];
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

    public bool Craft(Recipe recipe)
    {
        if (recipe.MaterialItemId1 != -1) // 레시피 아이템이 설정됨
        {
            if (CountByID[recipe.MaterialItemId1] < recipe.MaterialItemQuantity1) return false; //부족
        }
        if (recipe.MaterialItemId2 != -1) 
        {
            if (CountByID[recipe.MaterialItemId2] < recipe.MaterialItemQuantity2) return false;
        }
        if (recipe.MaterialItemId3 != -1) 
        {
            if (CountByID[recipe.MaterialItemId3] < recipe.MaterialItemQuantity3) return false;
        }
        if (recipe.MaterialItemId4 != -1) 
        {
            if (CountByID[recipe.MaterialItemId4] < recipe.MaterialItemQuantity4) return false;
        }
        RemoveItemByID(recipe.MaterialItemId1, recipe.MaterialItemQuantity1);
        RemoveItemByID(recipe.MaterialItemId2, recipe.MaterialItemQuantity2);
        RemoveItemByID(recipe.MaterialItemId3, recipe.MaterialItemQuantity3);
        RemoveItemByID(recipe.MaterialItemId4, recipe.MaterialItemQuantity4);

        AddItemByID(recipe.ResultItemId, recipe.ResultQuantity, GetMaxDur(recipe.ResultItemId));
        return true;

    }




}
