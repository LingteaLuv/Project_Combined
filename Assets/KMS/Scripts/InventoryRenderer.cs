using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using static Codice.CM.Common.CmCallContext;

public class InventoryRenderer : MonoBehaviour
{
    [SerializeField] private InventoryModel _model;

    private void Start()
    {
        RenderInventory();
        SelectRender(-1, -1);
        HoldClear();
    }

    /// <summary>
    /// 모델 내에 배열 형태로 저장된 아이템의 정보를 UI 상에 띄움.
    /// </summary>
    public void RenderInventory()
    {
        for (int i = 0; i < _model.SlotCount; i++)
        {
            if (_model.InvItems[i] == null)
            {
                _model.InvSlotItemImages[i].enabled = false;
                _model.InvSlotItemAmountTexts[i].enabled = false;
                _model.InvSlotItemDurSliders[i].gameObject.SetActive(false);
                continue;
            }
            if (i == InventoryManager.Instance.Controller.HoldingIndex && InventoryManager.Instance.Controller.IsHolding) continue;
            _model.InvSlotItemImages[i].enabled = true;
            _model.InvSlotItemImages[i].sprite = _model.InvItems[i].Data.Sprite;
            if (_model.InvItems[i].StackCount > 1)
            {
                _model.InvSlotItemAmountTexts[i].enabled = true;
                _model.InvSlotItemAmountTexts[i].text = _model.InvItems[i].StackCount.ToString();
            }
            else _model.InvSlotItemAmountTexts[i].enabled = false;

            if (_model.InvItems[i].MaxDurability != -1)
            {
                _model.InvSlotItemDurSliders[i].gameObject.SetActive(true);
                _model.InvSlotItemDurSliders[i].value = (float)_model.InvItems[i].Durability / _model.InvItems[i].MaxDurability;

            }
            else _model.InvSlotItemDurSliders[i].gameObject.SetActive(false);
        }
    }

    public void HoldClear()
    {
        _model.HoldSlotItemImage.enabled = false;
        _model.HoldSlotItemAmountText.enabled = false;
    }

    public void HoldRender(int index)
    {
        _model.HoldSlotItemImage.enabled = true;
        _model.HoldSlotItemImage.sprite = _model.InvSlotItemImages[index].sprite;
        if (_model.InvItems[index].Data.Type == ItemType.ETC)
        {
            _model.HoldSlotItemAmountText.enabled = true;
            _model.HoldSlotItemAmountText.text = _model.InvItems[index].StackCount.ToString();
        }
        _model.InvSlotItemImages[index].enabled = false;
        _model.InvSlotItemAmountTexts[index].enabled = false;
        _model.InvSlotItemDurSliders[index].gameObject.SetActive(false);
    }

    public void SelectRender(int before, int current) //선택된 칸, 쓰레기버튼
    {
        if (before != -1)
        {
            _model.InvSlotPanelImages[before].color = _model.SlotColor;
        }
        if (current != -1)
        {
            _model.InvSlotPanelImages[current].color = new Color(1f, 0f, 0f);
            _model.TrashButton.SetActive(true);
        }
        else
        {
            _model.TrashButton.SetActive(false);
        }
        RenderDescription(current);
        RenderUtilButton(current);
    }

    public void RenderDescription(int index)
    {
        if (index == -1)
        {
            _model.Desc.enabled = false;
            return;
        }
        _model.Desc.enabled = true;
        string str = $"Name:{_model.InvItems[index].Data.Name}\nDesc:{_model.InvItems[index].Data.Description}";
        _model.Desc.text = str;
    }

    public void RenderUtilButton(int index)
    {
        if (index == -1)
        {
            _model.UtilButton.SetActive(false);
            return;
        }
        ItemType type = _model.InvItems[index].Data.Type;
        if (type == ItemType.Material || type == ItemType.ETC)
        {
            _model.UtilButton.SetActive(false);
            return;
        }
        _model.UtilButton.SetActive(true);
        if (type == ItemType.Gun || type == ItemType.Melee || type == ItemType.Special || type == ItemType.Shield)
        {
            _model.UtilButtonText.text = "Equip";
        }
        else
        {
            _model.UtilButtonText.text = "Use (TODO)";
        }

    }

    public void RenderEquip(int[] indices)
    {
        for (int i = 0; i < 6; i++)
        {
            if (i == indices[0] || i == indices[1])
            {
                _model.InvSlotPanelImages[i].color = new Color(0f, 1f, 0f);
            }
            else
            {
                _model.InvSlotPanelImages[i].color = _model.SlotColor;
            }
        }
    }

    public void UpdateDur(int i) // 착용된 슬롯 내구도만 업데이트
    {
        _model.InvSlotItemDurSliders[i].value = (float)_model.InvItems[i].Durability / _model.InvItems[i].MaxDurability;
    }
}
