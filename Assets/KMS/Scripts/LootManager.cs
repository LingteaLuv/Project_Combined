using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootManager : Singleton<LootManager>
{

    [SerializeField] private GameObject _f;
    [SerializeField] private GameObject _lootTable;

    private int _slotCount = 6;
    private Image[] _itemImages;
    private TMP_Text[] _itemCountTexts;
    private Slider[] _itemDurSliders;

    private Lootable _lootable = null;

    private Stack<GameObject> _stack;
    private void Awake()
    {
        SetInstance();
        _stack = new Stack<GameObject>();
    }

    private void Init()
    {
        _itemImages = new Image[_slotCount];
        _itemCountTexts = new TMP_Text[_slotCount];
        _itemDurSliders = new Slider[_slotCount];
        for (int i = 0; i < _slotCount; i++)
        {
            //_itemImages[i] = _inventorySlots[i].gameObject.GetComponentsInChildren<Image>()[1];
            //_itemCountTexts[i] = _inventorySlots[i].gameObject.GetComponentInChildren<TMP_Text>();
        }

    }

    public void ClearUI()
    {
        _stack.Clear();
        _f.SetActive(false);
        _lootTable.SetActive(false);
    }

    public void NewLootableChecked(Lootable _lootable)
    {
        this._lootable = _lootable;
        ClearUI();
        _stack.Push(_f);
        SetUI();
    }

    public void SetUI()
    {
        if (_stack == null) return;
        foreach( GameObject s in _stack)
        {
            if (s == _stack.Peek())
            {
                s.SetActive(true);
            }
            else
            {
                s.SetActive(false);
            }
        }
    }

    public void LootableNotExist()
    {
        ClearUI();
    }

    public void OpenLootTable()
    {
        _stack.Push(_lootTable);
        SetUI();
        LootTableUpdate();
    }
    private void LootTableUpdate()
    {

    }
}
