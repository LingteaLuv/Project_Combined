using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilButtonController : MonoBehaviour
{
    public void TryEquip()
    {
        InventoryManager.Instance.Controller.UseETCItemButton(InventoryManager.Instance.Controller.SelectedIndex);
        InventoryManager.Instance.Controller.EquipButton(InventoryManager.Instance.Controller.SelectedIndex);
    }
}
