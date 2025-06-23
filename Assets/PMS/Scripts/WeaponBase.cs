using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//최상위 부모
public abstract class WeaponBase : MonoBehaviour
{
    private float _weaponBaseid; //

    public abstract void Attack();
}
