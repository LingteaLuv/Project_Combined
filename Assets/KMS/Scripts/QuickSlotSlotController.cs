 
using UnityEngine;

public class QuickSlotSlotController : InvSlotController
{
    public void TryEquip()
    {
        int index = GetSlotIndex();
        InventoryManager.Instance.Controller.Equip(index);

    }
}
