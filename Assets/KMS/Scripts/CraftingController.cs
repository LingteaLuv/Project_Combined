using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;

public class CraftingController : MonoBehaviour
{

    public Dictionary<int, int> CountByID { get; set; } //id를 통해 현재 가진 아이템의 개수를 가져오기 위함
    public Dictionary<int, ItemBase> ItemListByID { get; set; } //id를 통해 itembase를 불러오기 위함
    public ItemListSO ItemList { get; set; }

    public InventoryController Controller { get; }

    private void Awake()
    {
        CountByID = new Dictionary<int, int>();
        Init();
    }
    private void Init()
    {
        foreach (ItemBase i in ItemList.ItemList)
        {
            ItemListByID.Add(i.ItemID, i);
        }
    }

    public void Add(int id, int count)
    {
        if (CountByID.ContainsKey(id)) //있거나, 0개면
        {
            CountByID[id] += count;
        }
        else // 없으면 새로 추가
        {
            CountByID.Add(id, count);
        }
    }

    public bool AddItemByID(int id, int count, int dur)
    {
        if (ItemListByID.TryGetValue(id, out ItemBase item))
        {
            return Controller.AddItem(item, count, dur);
        }
        return false;
    }
    public bool RemoveItemByID(int id, int count)
    {
        if (ItemListByID.TryGetValue(id, out ItemBase item))
        {
            return Controller.RemoveItem(item, count);
        }
        return false;
    }


}
