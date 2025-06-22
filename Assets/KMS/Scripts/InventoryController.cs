using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.XR;
using UnityEngine.UIElements;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryModel _model;

    [SerializeField] private InventoryRenderer _renderer;

    public bool IsHolding;
    public int HoldingIndex;
    public int NextIndex;

    public int SelectedIndex;
    private int _beforeSelectedIndex;

    private void Awake()
    {
        IsHolding = false;
        HoldingIndex = -1;
        NextIndex = -1;
        SelectedIndex = -1;
        _beforeSelectedIndex = -1;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) InventoryManager.Instance.ToggleInventory();
        if (Input.GetKeyDown(KeyCode.Alpha0)) AddItem(_model.ItemList.ItemList[0]);
        if (Input.GetKeyDown(KeyCode.Alpha1)) AddItem(_model.ItemList.ItemList[1]);
        if (Input.GetKeyDown(KeyCode.Alpha2)) AddItem(_model.ItemList.ItemList[2]);
        if (Input.GetKeyDown(KeyCode.Alpha3)) AddItem(_model.ItemList.ItemList[3]);
        InventoryManager.Instance.HoldSlot.transform.position = Input.mousePosition;
    }

    public bool AddItem(ItemSO item)
    {
        int nullindex = -1;
        for (int i = 0; i < _model.SlotCount; i++)
        {
            if (_model.InvItems[i] == item && _model.InvItemAmounts[i] < item.MaxInventoryAmount)
            {
                _model.InvItemAmounts[i]++;
                if (InventoryManager.Instance.IsinventoryOpened) _renderer.RenderInventory();
                return true;
            }
            if (_model.InvItems[i] == null && nullindex == -1)
            {
                nullindex = i;
            }
        }
        if (nullindex != -1)
        {
            _model.InvItems[nullindex] = item;
            _model.InvItemAmounts[nullindex]++;
            if (InventoryManager.Instance.IsinventoryOpened) _renderer.RenderInventory();
            return true;
        }
        return false;
    }

    public void RemoveItem(ItemSO item)
    {
        for (int i = 0; i < _model.SlotCount; i++)
        {
            if (_model.InvItems[i] == item)
            {
                RemoveItemByIndex(i);
                return;
            }
        }
    }

    public void RemoveItemByIndex(int index)
    {
        _model.InvItemAmounts[index]--;
        if (_model.InvItemAmounts[index] <= 0)
        {
            _model.InvItemAmounts[index] = 0;
            _model.InvItems[index] = null;
        }
        if (InventoryManager.Instance.IsinventoryOpened) _renderer.RenderInventory();
    }

    //public void HandleItem(int index)
    //{
    //    if (_model.HeldItem == null)
    //    {
    //        if (_model.InvItems[index] == null) return;
    //        HoldItem(index);
    //    }
    //    else
    //    {
    //        if (_model.HeldItem == _model.InvItems[index] && 
    //            _model.InvItemAmounts[index] < _model.InvItems[index].MaxInventoryAmount)
    //        {
    //            
    //            CombineItem(index);
    //        }
    //        else if (_model.InvItems[index] == null)
    //        {
    //            PlaceItem(index);
    //        }
    //        else
    //        {
    //            ReplaceItem(index);
    //        }
    //    }
    //    _renderer.RenderInventory();
    //}

    public void HoldItem(int index)
    {
        if (_model.InvItems[index] == null) return;
        Debug.Log(HoldingIndex);
        _renderer.HoldRender(index);
        IsHolding = true;
        HoldingIndex = index;
        NextIndex = HoldingIndex;
    }

    public void CancelHolding()
    {
        if (IsHolding)
        {
            IsHolding = false;
            HoldingIndex = -1;
            NextIndex = -1;
            _renderer.HoldClear();
        }
    }
    //public void CombineItem(int index)
    //{
    //    int sum = _model.HeldItemAmount + _model.InvItemAmounts[index];
    //    if (sum <= _model.InvItems[index].MaxInventoryAmount)
    //    {
    //        _model.InvItemAmounts[index] = sum;
    //        _model.HeldItemAmount = 0;
    //        _model.HeldItem = null;
    //    }
    //    else
    //    {
    //        _model.InvItemAmounts[index] = _model.InvItems[index].MaxInventoryAmount;
    //        _model.HeldItemAmount = sum - _model.InvItems[index].MaxInventoryAmount;
    //    }
    //}

    public void SelectSlot(int index)
    {
        _beforeSelectedIndex = SelectedIndex;
        SelectedIndex = index;
        _renderer.SelectRender(_beforeSelectedIndex, SelectedIndex);


    }
    public void PutItem()
    {
        if (!IsHolding) return;
        Debug.Log("put");
        if (NextIndex == HoldingIndex) ;
        else if (_model.InvItems[NextIndex] == null)
        {
            PlaceItem();
            Debug.Log("place");
        }
        else
        {
            SwitchItem();
            Debug.Log("replace");
        }
        IsHolding = false;
        HoldingIndex = -1;
        NextIndex = -1;
        _renderer.HoldClear();
        _renderer.RenderInventory();
    }

    public void PlaceItem()
    {
        _model.InvItems[NextIndex] = _model.InvItems[HoldingIndex];
        _model.InvItemAmounts[NextIndex] = _model.InvItemAmounts[HoldingIndex];
        _model.InvItemDurabilitys[NextIndex] = _model.InvItemDurabilitys[HoldingIndex];
        _model.InvItems[HoldingIndex] = null;
        _model.InvItemAmounts[HoldingIndex] = 0;
        _model.InvItemDurabilitys[HoldingIndex] = 0;
    }
    public void SwitchItem()
    {
        ItemSO tempItem = _model.InvItems[NextIndex];
        int tempAmount = _model.InvItemAmounts[NextIndex];
        int tempDur = _model.InvItemDurabilitys[NextIndex];
        _model.InvItems[NextIndex] = _model.InvItems[HoldingIndex];
        _model.InvItemAmounts[NextIndex] = _model.InvItemAmounts[HoldingIndex];
        _model.InvItemDurabilitys[NextIndex] = _model.InvItemDurabilitys[HoldingIndex];
        _model.InvItems[HoldingIndex] = tempItem;
        _model.InvItemAmounts[HoldingIndex] = tempAmount;
        _model.InvItemDurabilitys[HoldingIndex] = tempDur;
    }
}
 