using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSlotController : MonoBehaviour
{
    public void TryGet()
    {
        LootManager.Instance.GetItem(GetSlotIndex());
    }
    private int GetSlotIndex()
    {
        int.TryParse(gameObject.name, out int index);
        return index;
    }
}
