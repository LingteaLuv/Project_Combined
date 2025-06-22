using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Items/Shield")]
public class ShieldItem : ItemBase
{
    public int MaxDurability;
    public int DefenseAmount;
}
