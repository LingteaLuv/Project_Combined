using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    private HingeDoorOpen _door;

    private void Awake()
    {
        _door = GetComponent<HingeDoorOpen>();
    }

    public void Interact()
    {
        _door.Toggle();
    }
}

