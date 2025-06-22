using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKey", menuName = "Key/KeyData")]
public class Key : ScriptableObject
{
    public string KeyId;
    public Sprite Icon;
    public string Description;
}
