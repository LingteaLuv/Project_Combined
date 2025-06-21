using UnityEngine;

/// <summary>
/// 입력을 기반으로 실제 이동 처리
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;

    [Header("Move Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpAccel = 2f;
    [SerializeField] private float _moveAccel = 15f;

    [SerializeField] private PlayerInputHandler _input;

    private Vector3 _currentInput = Vector3.zero;
    private float _curAccel;
    private bool _isJumped;

    /// <summary>
    /// 초기화. 리지드바디 캐시 및 입력 이벤트 등록
    /// </summary>
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _input.MoveInput.OnChanged += input => _currentInput = input;
    }

    /// <summary>
    /// 파괴 시 이벤트 해제
    /// </summary>
    private void OnDestroy()
    {
        _input.MoveInput.OnChanged -= input => _currentInput = input; // 람다는 그대로 두면 해제 안 되니까 주의
    }

    /// <summary>
    /// 물리 업데이트에서 이동 처리
    /// </summary>
    private void FixedUpdate()
    {
        Move(_currentInput);
    }

    /// <summary>
    /// 이동 처리 로직
    /// </summary>
    /// <param name="dir">입력 방향</param
    private void Move(Vector3 dir)
    {
        // 입력이 없는 경우 이동 정지
        if (dir == Vector3.zero)
        {
            Vector3 stop = new Vector3(0f, _rb.velocity.y, 0f);
            _rb.velocity = stop;
            return;
        }
        // 카메라 기준으로 이동 방향 설정
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("Main Camera not found. Tag a camera as 'MainCamera'.");
            return;
        }

        Vector3 moveDir = cam.transform.forward * dir.z + cam.transform.right * dir.x;
        moveDir.y = 0f;
        moveDir.Normalize();
        // 점프 여부에 따라 가속도 설정
        _curAccel = _isJumped ? _jumpAccel : _moveAccel;

        Vector3 velocity = moveDir * _moveSpeed;
        velocity.y = _rb.velocity.y;

        // 즉각적인 속도 반영
        _rb.velocity = velocity;
    }
}