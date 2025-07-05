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
        if (_outlinable == null)
            return;
      
        if (enable && _door != null && !_door.CanOpen())
        {
            SetOutline(true, Color.red);
            return;
        }
        SetOutline(enable, Color.yellow);
    }

    private void SetOutline(bool enable, Color? color = null)
    {
        if (_outlinable != null)
        {
            _outlinable.enabled = enable;
            if (color.HasValue)
            {
                Debug.Log($"[Outline] 색 바꿈: {color.Value}");
                _outlinable.FrontParameters.Color = color.Value;
                _outlinable.BackParameters.Color = color.Value;
            }
        }
    }

}