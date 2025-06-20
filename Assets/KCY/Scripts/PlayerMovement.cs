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

    // ���� Ȯ�� 
    [SerializeField]private Transform _jumpCheck; // �迭�� ��ȯ�ϰ�, ��ü Ȯ���ؼ� ���� ������Ʈ�� �����ϵ���
    [SerializeField]private LayerMask _groundMask;
    private float _jumpDis = 0.3f;
    


    // ���� ���� ����
    private float _curAccel;
    // ���� Ȯ��
    private bool _isJumped = false;
    



    public void SetMove(Vector3 dir)
    {
        // �� �ӵ� Ȯ��
        Vector3 move = Rb.velocity;
        // ���� �ӵ�
        Vector3 vec = dir * _moveSpeed;

        // ���� �� ���� ���ӵ��� ��ȯ�ϰ� ���� �� �̵��ϴ� �ӵ��� �����Ѵ�.
        _curAccel = _isJumped ? _jumpAccel : _moveAccel;
        move.x = Mathf.MoveTowards(move.x, vec.x, _curAccel * Time.deltaTime);
        move.z = Mathf.MoveTowards(move.z, vec.z, _curAccel * Time.deltaTime);

        // ���� �ӵ��� �ݿ�
        Rb.velocity = move; 
    }
    public void Jump()
    {
        // 2�� ���� ����
        if (_isJumped == true) return;

        // ����
        Rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        _isJumped = true;

        // �����ϸ鼭 �������� �� �̵��ӵ��� �������� ���� ����
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
