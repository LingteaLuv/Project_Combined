using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Items/SpecialItem")]
public class SpecialItem : ItemBase
{
    public int MaxDurability;
    public int DurabilitySec;
    public AudioClip SoundResource;
}
