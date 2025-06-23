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
    public FUIController FUICon { get; set; }


    private void Awake()
    {
        OffOutline();
        FUICon = GetComponent<FUIController>();
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
