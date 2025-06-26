using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private List<ItemBase> testKeys;
    private HingeDoorOpen _door;

    private void Awake()
    {
        _door = GetComponent<HingeDoorOpen>();
    }

    public void Interact()
    {
        _door.Toggle(testKeys);
    }
}

