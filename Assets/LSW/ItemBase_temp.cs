using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase_temp : ScriptableObject
{
    public int ItemId;
    public string Name;
    public string Description;
    public ItemType ItemType;
    public Sprite Icon;
}

public enum ItemType
{
    Weapon, Shield, Special, Consumable, ETC, Stuff
}
