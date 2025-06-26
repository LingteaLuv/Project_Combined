
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "ScriptableObjects/ItemList", order = 2)]
public class ItemListSO : ScriptableObject
{
    [SerializeField] private List<ItemBase> _itemList;

    public List<ItemBase> ItemList { get { return _itemList; }}
}
