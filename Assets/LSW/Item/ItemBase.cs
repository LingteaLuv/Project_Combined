using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemBase : ScriptableObject
{
    public int ItemID;
    public string Name;
    public string Description;
    public ItemType Type;
    public Sprite Sprite;
}

public enum ItemType
{
    Melee, Gun, Shield, Special, Consumable, ETC, Throw, Material
}
