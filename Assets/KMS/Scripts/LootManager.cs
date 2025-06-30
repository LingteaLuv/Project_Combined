
using System;
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

    public int CurrentBlockerIndex;
    private SlotBlockerController[] _slotBlockers;
    private LootSlotController[] _lootSlots;

    private Image[] _itemImages;
    private TMP_Text[] _itemCountTexts;
    private Slider[] _itemDurSliders;

    private Lootable _lootable = null;
    private void Awake()
    {
        SetInstance();
        
        Init();

    }

    private void Init()
    {
        CurrentBlockerIndex = -1;
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
    public void ToggleUI() //UI열기
    {
        UIManager.Instance.ToggleUI(ModalUI.lootTable);
        LootTableUpdate();

    }

    public void CancelBlockHolding()
    {
        if (UIManager.Instance.Current != ModalUI.lootTable) return;
        if (CurrentBlockerIndex == -1) return;
        _slotBlockers[CurrentBlockerIndex].PointerUp();
    }

    public void NewLootableChecked(Lootable _lootable)
    {
        this._lootable = _lootable;
    }

    public void LootableNotExist()
    {
        if (UIManager.Instance.Current == ModalUI.lootTable)
        {
            UIManager.Instance.CloseUI();
        }
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

            if (_lootable.LootItems.Items[i].MaxDurability != -1)
            {
                _itemDurSliders[i].gameObject.SetActive(true);
                _itemDurSliders[i].value = (float)_lootable.LootItems.Items[i].Durability / _lootable.LootItems.Items[i].MaxDurability;

            }
            else _itemDurSliders[i].gameObject.SetActive(false);
        }
    }

    public void Pickup()
    {
        int count = 0;
        for (int i = 0; i < 6; i++)
        {
            if (_lootable.LootItems.Items[i] != null) count++;
        }
        for (int i = 0; i < count; i++)
        {
            GetItem(i);
        }
    }
    public void GetItem(int index) //UI내 아이템 클릭 시 호출
    {
        if (_lootable == null) return;
        if (_lootable.LootItems.Items[index] == null) return;
        ItemBase data = _lootable.LootItems.Items[index].Data;
        int count = _lootable.LootItems.Items[index].StackCount;
        int dur = _lootable.LootItems.Items[index].Durability;
        if (InventoryManager.Instance.AddItem(data, count, dur)) // 해당 아이템을 넣을 수 있다면 넣어준다
        {
           _lootable.LootItems.Items[index] = null; // 아이템 빼기
           if (UIManager.Instance.Current == ModalUI.lootTable)
            {
                LootTableUpdate();
            }
           if (ItemsAllNull()) // 더이상 루팅 가능한 아이템이 없을 경우
            {
                AfterLooting();
            }
        }
    }

    public void AfterLooting()
    {
        _lootable.IsLootable = false;
        if (UIManager.Instance.Current == ModalUI.lootTable) ToggleUI();
        if (_lootable.DestroyAfterLooting) // 루팅 완료 시 파괴
        {
            Lootable temp = _lootable;
            if (temp.After != null) //다음에 전환할 것이 있음
            {
                Vector3 posOffset = temp.transform.root.position + Vector3.up * 0.23f;
                GameObject g = Instantiate(temp.After, posOffset, temp.transform.root.rotation);
                g.SetActive(true);
            }
            Destroy(temp.transform.root.gameObject);
            _lootable = null;
            Debug.Log("destroy");
            return;
        }
        _lootable = null;
    }

    public bool ItemsAllNull()
    {
        for (int i = 0; i < 6; i++)
        {
            if (_lootable.LootItems.Items[i] != null) return false;
        }
        return true;
    }
}
