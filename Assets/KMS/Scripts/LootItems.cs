using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItems : MonoBehaviour
{

    public ItemSO[] Items = new ItemSO[6];
    public int[] ItemAmounts = new int[6];
    public int[] ItemDurabilitys = new int[6];

    private void Awake()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == null) continue;
            if (Items[i].MaxDurability == -1) 
            { 
                ItemDurabilitys[i] = -1;
                continue;
            } 
            float a = UnityEngine.Random.Range(0.6f, 1.0f);
            ItemDurabilitys[i] = (int)(Items[i].MaxDurability * a);
        }
    }
}
