using UnityEngine;

/// <summary>
/// 입력을 기반으로 실제 이동 처리
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public Property<Vector3> MoveInput { get; private set; } = new(Vector3.zero);

    private Rigidbody _rb;
    private PlayerStateMachine _fsm;

    [Header("Move Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 5f;
    public float JumpForce => _jumpForce;
    public Rigidbody Rigidbody => _rb;

    public bool IsGrounded => Physics.Raycast(transform.position, Vector3.down, 1.1f);
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _fsm = new PlayerStateMachine();
        _fsm.ChangeState(new PlayerIdleState(_fsm, this));
    }

    private void Update()
    {
        Vector3 input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0f,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        MoveInput.Value = input;

        _fsm.Update();
    }

    /// <summary>
    /// 카메라 기준 방향으로 이동 처리
    /// </summary>
    /// <param name="dir">이동 방향</param>
    public void Move(Vector3 dir)
    {
        if (dir == Vector3.zero)
        {
            Vector3 stop = new Vector3(0f, _rb.velocity.y, 0f);
            _rb.velocity = stop;
            return;
        }

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("Main Camera not found.");
            return;
        }

        Vector3 moveDir = cam.transform.forward * dir.z + cam.transform.right * dir.x;
        moveDir.y = 0f;
        moveDir.Normalize();

        Vector3 velocity = moveDir * _moveSpeed;
        velocity.y = _rb.velocity.y;

        _rb.velocity = velocity;
    }
}