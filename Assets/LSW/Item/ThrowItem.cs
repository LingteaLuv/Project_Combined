using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ThrowItem")]
public class ThrowItem : ItemBase
{
    public int MaxStack;
    public int AtkDamage;
    public int AtkSpeed;
    public string ThrowSoundResource;
}
