using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//최상위 부모
public abstract class WeaponBase : MonoBehaviour
{
    [Header("Weapon Base Setting")]
    private float _weaponBaseid; //
    public abstract bool IsAttack { get; }

    public abstract void Attack();
}
