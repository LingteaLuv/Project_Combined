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
        HoldClear();
    }

    /// <summary>
    /// 모델 내에 배열 형태로 저장된 아이템의 정보를 UI 상에 띄움.
    /// </summary>
    public void RenderInventory()
    {
        //for (int i = 0; i < _model.SlotCount; i++)
        //{
        //    if (_model.InvItems[i] != null)
        //    {
        //        _model.InvSlotItemImages[i].enabled = true;
        //        _model.InvSlotItemAmountTexts[i].enabled = true;
        //        _model.InvSlotItemImages[i].sprite = _model.InvItems[i].Sprite;
        //        _model.InvSlotItemAmountTexts[i].text = _model.InvItemAmounts[i].ToString();
        //    }
        //    else
        //    {
        //        _model.InvSlotItemImages[i].enabled = false;
        //        _model.InvSlotItemAmountTexts[i].enabled = false;
        //    }
        //}
        //if (_model.HeldItem != null)
        //{
        //    _model.HoldSlotItemImage.enabled = true;
        //    _model.HoldSlotItemAmountText.enabled = true;
        //    _model.HoldSlotItemImage.sprite = _model.HeldItem.Sprite;
        //    _model.HoldSlotItemAmountText.text = _model.HeldItemAmount.ToString();
        //}
        //else
        //{
        //    _model.HoldSlotItemImage.enabled = false;
        //    _model.HoldSlotItemAmountText.enabled = false;
        //}


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
            _model.InvSlotItemImages[i].sprite = _model.InvItems[i].Sprite;
            if (_model.InvItemAmounts[i] > 1)
            {
                _model.InvSlotItemAmountTexts[i].enabled = true;
                _model.InvSlotItemAmountTexts[i].text = _model.InvItemAmounts[i].ToString();
            }
            else _model.InvSlotItemAmountTexts[i].enabled = false;

            if (_model.InvItems[i].MaxDurability != -1)
            {
                _model.InvSlotItemDurSliders[i].gameObject.SetActive(true);
                _model.InvSlotItemDurSliders[i].value = (float)_model.InvItemDurabilitys[i] / _model.InvItems[i].MaxDurability;

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
        if (_model.InvItemAmounts[index] > 1)
        {
            _model.HoldSlotItemAmountText.enabled = true;
            _model.HoldSlotItemAmountText.text = _model.InvItemAmounts[index].ToString();
        }
        _model.InvSlotItemImages[index].enabled = false;
        _model.InvSlotItemAmountTexts[index].enabled = false;
        _model.InvSlotItemDurSliders[index].gameObject.SetActive(false);
    }

    public void SelectRender(int before, int current)
    {
        if (before == -1) ;
        else _model.InvSlotPanelImages[before].color = _model.SlotColor;
        if (current == -1) ;
        else _model.InvSlotPanelImages[current].color = new Color(1f, 0f, 0f);
    }
}
