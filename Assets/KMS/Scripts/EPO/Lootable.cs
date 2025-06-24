using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EPOOutline;

public class Lootable : MonoBehaviour
{
    [SerializeField] private Outlinable _outlinable;
    public LootItems LootItems;
    public Outlinable Outlinable { get { return _outlinable; } }

    //[SerializeField] private FUIController _FUIController;
    public FUIController FUIController { get; set; }

    public bool IsLootable { get; set; }


    private void Awake()
    {
        _outlinable = GetComponentInParent<Outlinable>();
        FUIController = GetComponent<FUIController>();
        OffOutline();
        IsLootable = false;
    }

    public void OnOutline()
    {
        _outlinable.enabled = true;
    }
    public void OffOutline()
    {
        _outlinable.enabled = false;
    }


}
