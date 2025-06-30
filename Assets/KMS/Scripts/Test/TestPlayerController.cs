using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestPlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    private PlayerAttack pa;
    public float moveSpeed = 4f;
    private void Update()
    {
        float r = Input.GetAxisRaw("Horizontal");
        float m = Input.GetAxisRaw("Vertical");

        transform.Rotate(Vector3.up, r * 0.5f);
        Vector3 moveDir = transform.forward * m;

        rigid.velocity = moveDir * moveSpeed;
    }
    private void Awake()
    {
        pa = GetComponent<PlayerAttack>();
    }
}
