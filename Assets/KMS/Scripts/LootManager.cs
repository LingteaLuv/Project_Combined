using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootManager : SingletonT<LootManager>
{

    [SerializeField] private GameObject _f;
    [SerializeField] private GameObject _lootTable;
    [SerializeField] private GameObject _slots;
    [SerializeField] private GameObject _blocks;


    public int SlotCount = 6;

    private SlotBlockerController[] _slotBlockers;
    private LootSlotController[] _lootSlots;

    private Image[] _itemImages;
    private TMP_Text[] _itemCountTexts;
    private Slider[] _itemDurSliders;

    private Lootable _lootable = null;

    private Stack<GameObject> _stack;
    private void Awake()
    {
        SetInstance();
        
        Init();
    }

    private void Init()
    {
        _stack = new Stack<GameObject>();
        _itemImages = new Image[SlotCount];
        _itemCountTexts = new TMP_Text[SlotCount];
        _itemDurSliders = new Slider[SlotCount];

        _lootSlots = _slots.GetComponentsInChildren<LootSlotController>();
        _slotBlockers = _blocks.GetComponentsInChildren<SlotBlockerController>();


        for (int i = 0; i < SlotCount; i++)
        {
            _itemImages[i] = _lootSlots[i].GetComponentsInChildren<Image>()[1];
            _itemCountTexts[i] = _lootSlots[i].GetComponentInChildren<TMP_Text>();
            _itemDurSliders[i] = _lootSlots[i].GetComponentInChildren<Slider>();
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

    public void RemoveBlocker(int index)
    {
        _lootable.LootItems.ItemBlocked[index] = false;
        LootTableUpdate();
    }
    private void LootTableUpdate()
    {
        for (int i = 0; i < SlotCount; i++)
        {
            if (_lootable.LootItems.ItemBlocked[i])
            {
                _slotBlockers[i].gameObject.SetActive(true);
                continue;
            }
            _slotBlockers[i].gameObject.SetActive(false);
            if (_lootable.LootItems.Items[i] == null)
            {
                _itemImages[i].enabled = false;
                _itemCountTexts[i].enabled = false;
                _itemDurSliders[i].gameObject.SetActive(false);
                continue;
            }
            _itemImages[i].enabled = true;
            _itemImages[i].sprite = _lootable.LootItems.Items[i].Data.Sprite;
            if (_lootable.LootItems.Items[i].StackCount > 1)
            {
                _itemCountTexts[i].enabled = true;
                _itemCountTexts[i].text = _lootable.LootItems.Items[i].StackCount.ToString();
            }
            else _itemCountTexts[i].enabled = false;

            if (_lootable.LootItems.Items[i].Durability != -1)
            {
                _itemDurSliders[i].gameObject.SetActive(true);
                _itemDurSliders[i].value = (float)_lootable.LootItems.Items[i].Durability / _lootable.LootItems.Items[i].Data.MaxDurability;

            }
            else _itemDurSliders[i].gameObject.SetActive(false);
        }
    }

    public void GetItem(int index)
    {
        ItemSO data = _lootable.LootItems.Items[index].Data;
        int count = _lootable.LootItems.Items[index].StackCount;
        int dur = _lootable.LootItems.Items[index].Durability;
        if (InventoryManager.Instance.AddItem(data, count, dur))
        {

        }
    }
}
