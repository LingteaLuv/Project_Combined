using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InvSlotController : MonoBehaviour
{
    [SerializeField] Button _slotButton;

    [SerializeField] private Image _hoverImage;

    public void OnEnable()
    {
        HoverImageDisable();
    }
    public void HoverImageEnable()
    {
        _hoverImage.enabled = true;
    }
    public void HoverImageDisable()
    {
        _hoverImage.enabled = false;
    }

    public void SlotClickEvent()
    {
        Debug.Log(1);
        int index = GetSlotIndex();
        InventoryManager.Instance.Controller.HandleItem(index);
    }

    private int GetSlotIndex()
    {
        int.TryParse(gameObject.name, out int index);
        return index;
    }
}
