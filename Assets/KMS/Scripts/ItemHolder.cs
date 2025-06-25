using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum HandType
{
    left,
    right
}
public class ItemHolder : MonoBehaviour
{
    [SerializeField] HandType type;

    public void Subscribe()
    {
        InventoryManager.Instance.Hand.Subscribe(type, this);
    }


}
