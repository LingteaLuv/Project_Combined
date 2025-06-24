using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActiveController : MonoBehaviour
{
    [Header("Drag&Drop")] 
    [SerializeField] private GameObject _bat;

    private void Awake()
    {
        Init();
    }
    
    public void EnableBat()
    {
        _bat.SetActive(true);
    }

    public void DisableBat()
    {
        _bat.SetActive(false);
    }

    private void Init()
    {
        _bat.SetActive(false);
    }
}
