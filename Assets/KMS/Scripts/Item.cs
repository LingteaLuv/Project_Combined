using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;

public class Item
{
    public ItemSO Data { get; private set; }
    public int StackCount { get; set; }
    public int Durability { get; set; }

    public Item(ItemSO data)
    {
        this.Data = data;
        this.StackCount = data.MaxInventoryAmount;
        this.Durability = data.MaxDurability;
    }
    public Item(ItemSO data, int count)
    {
        this.Data = data;
        this.StackCount = count;
        this.Durability = data.MaxDurability;
    }
    public Item(ItemSO data, int count, int dur)
    {
        this.Data = data;
        this.StackCount = count;
        this.Durability = dur;
    }
}
