using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Items/Etc")]
public class EtcItem : ItemBase
{
    public string EtcType;
    public int MaxStackSize;
    public string SoundResource;
    public string StrParam;
    public int IntParam;
}
