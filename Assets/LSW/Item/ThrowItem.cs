using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ThrowItem")]
public class ThrowItem : ItemBase
{
    public int MaxStack;
    public float AtkDamage;
    public float AtkSpeed;
    public float MinSpeed;
    public float MaxSpeed;
    public float MaxChargeTime;
    public float MinRotateValue;
    public float MaxRotateValue;
    public AudioClip ThrowSoundResource;
}
