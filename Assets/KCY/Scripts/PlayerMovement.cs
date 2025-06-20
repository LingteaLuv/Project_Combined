using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    // 내부에서만 사용하고 GetComponent로 가져오는 경우에는 private이 좋을 것 같아요! 
    private Rigidbody _rb;
    //private Animation Avatar;
    
    [Header("Move Element")]
    [SerializeField] private float _moveSpeed  = 5f;
    [SerializeField] private float _moveAccel = 15f;
    [SerializeField] private float _moveDecel = 30f;
    
    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private float _jumpAccel = 2f;
    [SerializeField] private float _rotSpeed = 1f;

    // ���� Ȯ�� 
    //[SerializeField]private Transform _jumpCheck; // �迭�� ��ȯ�ϰ�, ��ü Ȯ���ؼ� ���� ������Ʈ�� �����ϵ���
    //private float _jumpDis = 0.3f;

    // ���
    [SerializeField] private Transform _bottomRayOrigin;
    [SerializeField] private Transform _upperRayOrigin;
    [SerializeField]private LayerMask _groundMask;

    // ���� ���� ����
    private float _curAccel;
    // ���� Ȯ��
    private bool _isJumped;
    
    public bool IsGrounded { get; private set; }
    
    
    // 백뷰 시점 구현하기 위해 Camera 연동 코드 추가
    // Mathf.MoveToward -> Vector3.MoveToward로 통합
    // 감속 관련 코드 추가 (else 이하 부분)
    public void SetMove(Vector3 dir)
    {
        if (dir != Vector3.zero)
        { 
            Camera _playerFollowCam = Camera.main;
            Vector3 camForward = _playerFollowCam.transform.forward;
            Vector3 camRight = _playerFollowCam.transform.right;

            Vector3 moveDir = camForward * dir.z + camRight * dir.x;
            // �� �ӵ� Ȯ��
            // Vector3 move = _rb.velocity;
            // ���� �ӵ�
            Vector3 vec = moveDir * _moveSpeed;

            _curAccel = _isJumped ? _jumpAccel : _moveAccel;
            
            vec.y = _rb.velocity.y;
            
            _rb.velocity = Vector3.MoveTowards(_rb.velocity, vec, _curAccel * Time.fixedDeltaTime);
            
            // ���� �� ���� ���ӵ��� ��ȯ�ϰ� ���� �� �̵��ϴ� �ӵ��� �����Ѵ�.
            // move.x = Mathf.MoveTowards(move.x, vec.x, _curAccel * Time.deltaTime);
            // move.z = Mathf.MoveTowards(move.z, vec.z, _curAccel * Time.deltaTime);

            // ���� �ӵ��� �ݿ�
            // _rb.velocity = move; 
        }
        else
        {
            Vector3 targetV = Vector3.zero;
            targetV.y = _rb.velocity.y;
            _rb.velocity = Vector3.MoveTowards(_rb.velocity, targetV, _curAccel * Time.fixedDeltaTime);
        }
    }
    public void Jump()
    {
        // 2�� ���� ����
        if (_isJumped) return;

        // ����
        _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        _isJumped = true;

        // �����ϸ鼭 �������� �� �̵��ӵ��� �������� ���� ����
        Vector3 jumpVel = _rb.velocity;
        jumpVel.x *= 0.65f;
        jumpVel.z *= 0.65f;
        _rb.velocity = jumpVel;
    }

    public void ClimbStairs()
    {
        // ������ �÷��̾��� �����⿡ ���� ���� ��� �Ʒ��� ���̴� ������, ���� ���̿����� ������ �ʴ� ��� ������� �ν�
        Vector3 stairsRay = Vector3.forward;
        bool bottomRay = Physics.Raycast(_bottomRayOrigin.position, stairsRay, 0.4f, _groundMask);
        bool upperRay = Physics.Raycast(_bottomRayOrigin.position, stairsRay, 0.4f, _groundMask);

        // ������� �ν��ϴ� �Ϳ� �������� ��� upper���̰� �ִ� �� ��ŭ �÷���
        if (bottomRay && !upperRay)
        {
            //Rb.position += 
        }
    }
    
    public void GetIsGrounded(bool value)
    {
        IsGrounded = value;
    }

    public void GetIsJumped(bool value)
    {
        _isJumped = !value;
    }
    
    public void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * 5f);
    }
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        //Avatar = GetComponent<Animation>();
        IsGrounded = true;
    }
    
    private void Update()
    {
        // 바닥의 종류가 많아서 OnCollisionEnter로 처리하는건 어떨까 싶습니다!
        //_isJumped = !Physics.CheckSphere(_jumpCheck.position, _jumpDis, _groundMask);
    }
}
