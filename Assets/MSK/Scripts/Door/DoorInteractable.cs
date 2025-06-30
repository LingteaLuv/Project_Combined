using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemBase testKeys; // 문 자체가 열쇠를 가지도록 테스트
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

