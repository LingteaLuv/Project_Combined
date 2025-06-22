using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class LootItems : MonoBehaviour
    {

        public ItemSO[] ItemDatas = new ItemSO[6];
        public int[] ItemAmounts = new int[6];
        public int[] ItemDurabilitys = new int[6];
        public bool[] ItemBlocked = new bool[6];

        public Item[] Items;

        private void Awake()
        {
            Items = new Item[6];
            for (int i = 0; i < Items.Length; i++)
            {
                if (ItemDatas[i] == null) continue;
                ItemBlocked[i] = true;
                if (ItemDatas[i].MaxDurability == -1)
                {
                    ItemDurabilitys[i] = -1;
                }
                else
                {
                    float a = UnityEngine.Random.Range(0.6f, 1.0f);
                    ItemDurabilitys[i] = (int)(ItemDatas[i].MaxDurability * a);
                }
                Items[i] = new Item(ItemDatas[i], ItemAmounts[i], ItemDurabilitys[i]);
            }
        }
    }