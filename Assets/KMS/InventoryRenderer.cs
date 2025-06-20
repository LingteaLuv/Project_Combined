using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class InventoryRenderer : MonoBehaviour
{
    [SerializeField] private InventoryModel _model;

    private void Start()
    {
        RenderInventory();
    }

    /// <summary>
    /// �� ���� �迭 ���·� ����� �������� ������ UI �� ���.
    /// </summary>
    public void RenderInventory()
    {
        for (int i = 0; i < _model.slotCount; i++)
        {
            if (_model.InvItems[i] != null)
            {
                _model.InvSlotItemImages[i].enabled = true;
                _model.InvSlotItemAmountTexts[i].enabled = true;
                _model.InvSlotItemImages[i].sprite = _model.InvItems[i].Sprite;
                _model.InvSlotItemAmountTexts[i].text = _model.InvItemAmounts[i].ToString();
            }
            else
            {
                _model.InvSlotItemImages[i].enabled = false;
                _model.InvSlotItemAmountTexts[i].enabled = false;
            }
        }
        if (_model.HeldItem != null)
        {
            _model.HoldSlotItemImage.enabled = true;
            _model.HoldSlotItemAmountText.enabled = true;
            _model.HoldSlotItemImage.sprite = _model.HeldItem.Sprite;
            _model.HoldSlotItemAmountText.text = _model.HeldItemAmount.ToString();
        }
        else
        {
            _model.HoldSlotItemImage.enabled = false;
            _model.HoldSlotItemAmountText.enabled = false;
        }
    }
}
