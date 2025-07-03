using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Items/Melee")]
public class MeleeItem : ItemBase
{
    public int MaxDurability;
    public int AtkDamage;
    public int AtkSpeed;
    public AudioClip AtkSoundResources;
}
