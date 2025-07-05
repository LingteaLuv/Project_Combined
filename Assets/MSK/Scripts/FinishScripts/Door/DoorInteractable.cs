using EPOOutline;
using UnityEngine;

[RequireComponent(typeof(HingeDoorOpen))]
public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Test Door Init")]
    [SerializeField] private ItemBase testKeys;

    private GameObject outlineTarget;
    private HingeDoorOpen _door;
    private Outlinable _outlinable;

    private void Awake()
    {
        _door = GetComponent<HingeDoorOpen>();
        outlineTarget = _door._parentToRotate != null ? _door._parentToRotate.gameObject : null;

        if (outlineTarget != null)
            _outlinable = outlineTarget.GetComponent<Outlinable>();
        else
            _outlinable = GetComponent<Outlinable>();

        if (_outlinable != null)
            _outlinable.enabled = false;
    }

    public void Interact()
    {
        _door.Toggle(testKeys);
        SetOutline(false);
    }

    public void Highlight(bool enable)
    {
        // 열 수 없는 상태면 무조건 아웃라인 꺼짐
        if (enable && _door != null && !_door.CanOpen())
        {
            SetOutline(false);
            return;
        }
        SetOutline(enable);
    }

    private void SetOutline(bool enable)
    {
        if (_outlinable != null)
            _outlinable.enabled = enable;
    }
}