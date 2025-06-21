using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EPOOutline;

public class Lootable : MonoBehaviour
{
    [SerializeField] private Outlinable _outlinable;
    public Outlinable Outlinable { get { return _outlinable; } }

    [SerializeField] private LootItemSO _lootItems;

    public LootItemSO LootItemSO { get { return _lootItems; } }

    private void Awake()
    {
        OffOutline();
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
