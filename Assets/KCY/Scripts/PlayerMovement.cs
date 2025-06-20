using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Reference")]
    public Rigidbody Rb;
    //public Animation Avatar;


    [Header("Move Element")]
    [SerializeField] private float _moveSpeed  = 5f;
    [SerializeField] private float _moveAccel = 15f;
    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private float _jumpAccel = 2f;
    [SerializeField] private float _rotSpeed = 1f;

    // 점프 확인 
    [SerializeField]private Transform _jumpCheck; // 배열로 전환하고, 물체 확인해서 여러 오브젝트에 대응하도록
    [SerializeField]private LayerMask _groundMask;
    private float _jumpDis = 0.3f;
    


    // 현재 가속 저장
    private float _curAccel;
    // 점프 확인
    private bool _isJumped = false;
    



    public void SetMove(Vector3 dir)
    {
        // 현 속도 확인
        Vector3 move = Rb.velocity;
        // 설정 속도
        Vector3 vec = dir * _moveSpeed;

        // 점프 시 점프 가속도로 전환하고 점프 중 이동하는 속도를 조절한다.
        _curAccel = _isJumped ? _jumpAccel : _moveAccel;
        move.x = Mathf.MoveTowards(move.x, vec.x, _curAccel * Time.deltaTime);
        move.z = Mathf.MoveTowards(move.z, vec.z, _curAccel * Time.deltaTime);

        // 최종 속도를 반영
        Rb.velocity = move; 
    }
    public void Jump()
    {
        // 2단 점프 방지
        if (_isJumped == true) return;

        // 점프
        Rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        _isJumped = true;

        // 가속하면서 점프했을 때 이동속도가 빨라지는 것을 방지
        Vector3 jumpVel = Rb.velocity;
        jumpVel.x *= 0.65f;
        jumpVel.z *= 0.65f;
        Rb.velocity = jumpVel;
    }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        //Avatar = GetComponent<Animation>();
    }

    private void Update()
    {
        _isJumped = !Physics.CheckSphere(_jumpCheck.position, _jumpDis, _groundMask);
    }
}
