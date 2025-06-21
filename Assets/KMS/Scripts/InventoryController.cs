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

    private bool _isHolding;
    private int _holdingIndex;

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
        _renderer.HoldRender(index);
        _isHolding = true;
        _holdingIndex = index;
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
    //public void PlaceItem(int index)
    //{
    //    _model.InvItems[index] = _model.HeldItem;
    //    _model.InvItemAmounts[index] = _model.HeldItemAmount;
    //    _model.HeldItem = null;
    //    _model.HeldItemAmount = 0;
    //}
    //public void ReplaceItem(int index)
    //{
    //    ItemSO tempItem = _model.InvItems[index];
    //    int tempAmount = _model.InvItemAmounts[index];
    //    _model.InvItems[index] = _model.HeldItem;
    //    _model.InvItemAmounts[index] = _model.HeldItemAmount;
    //    _model.HeldItem = tempItem;
    //    _model.HeldItemAmount = tempAmount;
    //
    //}

    public void PutItem(int index)
    {
        if (!_isHolding) return;

        if (_model.InvItems[index] == null)
        {
            PlaceItem(index);
            Debug.Log("place");
        }
        else
        {
            ReplaceItem(index);
            Debug.Log("replace");
        }
        _renderer.HoldClear();
        _renderer.RenderInventory();
    }

    public void PlaceItem(int index)
    {
        _model.InvItems[index] = _model.InvItems[_holdingIndex];
        _model.InvItemAmounts[index] = _model.InvItemAmounts[_holdingIndex];
        _model.InvItemDurabilitys[index] = _model.InvItemDurabilitys[_holdingIndex];
    }
    public void ReplaceItem(int index)
    {
        ItemSO tempItem = _model.InvItems[index];
        int tempAmount = _model.InvItemAmounts[index];
        int tempDur = _model.InvItemDurabilitys[index];
        _model.InvItems[index] = _model.InvItems[_holdingIndex];
        _model.InvItemAmounts[index] = _model.InvItemAmounts[_holdingIndex];
        _model.InvItemDurabilitys[index] = _model.InvItemDurabilitys[_holdingIndex];
        _model.InvItems[_holdingIndex] = tempItem;
        _model.InvItemAmounts[_holdingIndex] = tempAmount;
        _model.InvItemDurabilitys[_holdingIndex] = tempDur;
    }
}
 