 
using UnityEngine;

public class PlayerInventoryData : MonoBehaviour
{
    public Item[] InvItems { get; private set; }

    private void Awake()
    {
        InvItems = new Item[18]; 
    }
}
