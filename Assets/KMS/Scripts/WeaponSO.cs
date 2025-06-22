using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 3)]
public class WeaponSO : ItemBase
{
    [SerializeField] private int _damage;
    public int Damage { get { return _damage; } }
}
