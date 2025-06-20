using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvSlotController : MonoBehaviour
{
    [SerializeField] Button _slotButton;


    public void SlotClickEvent()
    {
        int index = GetSlotIndex();
        //if (InventoryManager.Instance.Model.InvItems[index] == null)
    }

    private int GetSlotIndex()
    {
        int.TryParse(gameObject.name, out int index);
        return index;
    }
}
