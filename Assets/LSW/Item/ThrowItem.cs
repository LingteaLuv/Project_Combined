using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ThrowItem")]
public class ThrowItem : ItemBase
{
    public int AtkDamage;
    public int Rof;
    public float Range;
    public int MaxStack;
    public string ThrowSoundResource;
}
