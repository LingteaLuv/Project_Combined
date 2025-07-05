using EPOOutline;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(HingeDoorOpen))]
public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Test Door Init")]
    [SerializeField] private ItemBase testKeys;

    [Header("Optional Outline")]
    private GameObject outlineTarget;
    [SerializeField] private bool useOutline = true;
    [SerializeField] private AudioClip interactionSound;

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
        if (interactionSound != null)
            AudioSource.PlayClipAtPoint(interactionSound, transform.position, 1.0f);
        _door.Toggle(testKeys);
        SetOutline(false);
    }

    public void Highlight(bool enable)
    {   
        if (enable && _door != null && !_door.CanOpen())
        {
            SetOutline(false);
            return;
        }
        SetOutline(enable);
    }

    private void SetOutline(bool enable)
    {
        if (_outlinable != null && useOutline)
            _outlinable.enabled = enable;
    }
}