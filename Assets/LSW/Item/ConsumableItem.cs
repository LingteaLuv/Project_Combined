using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ConsumableItem")]
public class ConsumableItem : ItemBase
{
    public int HpAmount;
    public int MoistureAmount;
    public int HungerAmount;
    public string SoundResource;
}
