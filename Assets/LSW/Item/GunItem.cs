using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Gun")]
public class GunItem : ItemBase
{
    public int AtkDamage;
    public int Rof;
    public int BulletPerShot;
    public float Range;
    public int AmmoID;
    public int AmmoCapacity;
    public AudioClip ShotSoundResource;
    public AudioClip ReloadSoundResource;
    public float NoiseLevel;
}
