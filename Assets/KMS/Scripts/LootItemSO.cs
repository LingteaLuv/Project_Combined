using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootItem", menuName = "ScriptableObjects/LootItem")]
public class LootItemSO : ScriptableObject
{
    public List<ItemSO> Items;
    public List<int> Amounts;
}
