using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

/// <summary>
/// 입력을 감지하고 외부로 전달하는 클래스입니다.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    public Vector3 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool CrouchHeld { get; private set; }
    public bool IsOnLadder { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool RunPressed { get; private set; }
    public bool TestKey { get; private set; }

    [SerializeField] private LayerMask _doorLayerMask;
    
    private DoorInteractable _currentDoor;
    
    private void Awake()
    {
        _doorLayerMask = LayerMask.GetMask("Door");
    }
    
    private void DoorFind()
    {
        DoorInteractable door = null;
        // Raycast로 문 감지
        Ray ray = new Ray(transform.position + Vector3.up * 1.0f, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit doorHit, 1f,_doorLayerMask))
        {
            door = doorHit.collider.GetComponentInParent<DoorInteractable>();
            if (door == null)
                door = doorHit.collider.GetComponent<DoorInteractable>();
        }
        if (door != _currentDoor)
        {
            if (_currentDoor != null)
                _currentDoor.Highlight(false);

            if (door != null)
                door.Highlight(true);
            _currentDoor = door;
        }
    }
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        MoveInput = new Vector3(x, 0, z).normalized;

        JumpPressed = Input.GetButtonDown("Jump");
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        bool run = Input.GetKey(KeyCode.LeftShift);
        InteractPressed = Input.GetKeyDown(KeyCode.F);

        DoorFind();

        if (crouch && run)
        {
            CrouchHeld = true;
            RunPressed = false;
        }
        else if (crouch)
        {
            CrouchHeld = true;
            RunPressed = false;
        }
        else if (run)
        {
            CrouchHeld = false;
            RunPressed = true;
        }
        else
        {
            CrouchHeld = false;
            RunPressed = false;
        }

        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.forward, out RaycastHit ladderHit, 0.3f))
        {
            if (ladderHit.collider.CompareTag("Ladder"))
                IsOnLadder = true;
            else
                IsOnLadder = false;
        }
        else
        {
            IsOnLadder = false;
        }
    }
}