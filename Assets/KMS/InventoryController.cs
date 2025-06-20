using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.XR;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryModel _model;

    [SerializeField] private InventoryRenderer _renderer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ToggleInventory();
        if (Input.GetKeyDown(KeyCode.Alpha0)) AddItem(_model.ItemList.ItemList[0]);
        if (Input.GetKeyDown(KeyCode.Alpha1)) AddItem(_model.ItemList.ItemList[1]);
        if (Input.GetKeyDown(KeyCode.Alpha2)) AddItem(_model.ItemList.ItemList[2]);
    }

    public void ToggleInventory()
    {
        if (_model.Inventory.activeSelf)
        {
            _model.Inventory.SetActive(false);
        }
        else
        {
            _model.Inventory.SetActive(true);
            _renderer.RenderInventory();
        }
    }

    public bool AddItem(ItemSO item)
    {
        int nullindex = -1;
        for (int i = 0; i < _model.slotCount; i++)
        {
            if (_model.InvItems[i] == item && _model.InvItemAmounts[i] < item.MaxInventoryAmount)
            {
                _model.InvItemAmounts[i]++;
                if (_model.Inventory.activeSelf) _renderer.RenderInventory();
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
            if (_model.Inventory.activeSelf) _renderer.RenderInventory();
            return true;
        }
        return false;
    }

    public void RemoveItem(ItemSO item)
    {
        for (int i = 0; i < _model.slotCount; i++)
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
        _renderer.RenderInventory();
    }

    public void HoldItem()
    {

    }
    public void CombineItem()
    {

    }
    public void PlaceItem()
    {

    }
}
