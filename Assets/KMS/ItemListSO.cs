using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "ScriptableObjects/ItemList", order = 2)]
public class ItemListSO : ScriptableObject
{
    [SerializeField] private List<ItemSO> _itemList;

    public List<ItemSO> ItemList { get { return _itemList; }}
}
