using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemTypeT
{
    Normal,
    Weapon,
    Shield,
    Special,
    Consumable

}
[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemBaseLegacy : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    public Sprite Sprite { get { return _sprite; } }

    [SerializeField] private string _name;
    public string Name { get { return _name; } }

    [SerializeField] [TextArea] private string _description;
    public string Description { get { return _description; } }

    [SerializeField] private int _maxInventoryAmount;
    public int MaxInventoryAmount { get { return _maxInventoryAmount; } }

    [SerializeField] private int _maxDurability;
    public int MaxDurability { get { return _maxDurability; } }

    [SerializeField] private ItemType _type;
    public ItemType Type { get { return _type; } }


}
