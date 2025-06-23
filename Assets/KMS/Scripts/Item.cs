using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;

public class Item
{
    public ItemBase Data { get; private set; }
    public int StackCount { get; set; }
    public int Durability { get; set; }

    public int MaxDurability { get; private set; }
    public int MaxStackSize { get; private set; }
    public Item(ItemBase data)
    {
        Data = data;
        StackCount = 1;
        MaxStackSize = 1;
        Durability = -1;
        MaxDurability = -1;

        Init();
    }
    private void Init() // 각 클래스에 포함된 값들을 사용하기 쉽게 가져온다.
    {
        switch (Data.Type)
        {
            case ItemType.ETC:
                MaxStackSize = (Data as EtcItem).MaxStackSize;
                break;
            case ItemType.Melee:
                MaxDurability = (Data as MeleeItem).MaxDurability;
                break;
            case ItemType.Shield:
                MaxDurability = (Data as ShieldItem).MaxDurability;
                break;
            case ItemType.Special:
                MaxDurability = (Data as SpecialItem).MaxDurability;
                break;
        }
    }

    public void SetCount(int c)
    {
        if (Data.Type == ItemType.ETC) StackCount = c;
    }
    public void SetDur(int c)
    {
        if (Data.Type == ItemType.Melee || Data.Type == ItemType.Shield || Data.Type == ItemType.Special) Durability = c;
    }
}
