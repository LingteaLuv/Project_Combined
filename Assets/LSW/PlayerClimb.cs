using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    [Header("Drag&Drop")] 
    [SerializeField] private float _climbSpeed;
    
    public bool IsOnClimbed
    {
        get;
        private set;
    }
    
    private void Awake()
    {
        Init();
    }

    public void ClimbUpdate(bool isOnGround)
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f))
        {
            if(hit.collider.CompareTag("Ladder"))
            {
                IsOnClimbed = true;
                Climb(isOnGround);
            }
            else
            {
                IsOnClimbed = false; 
            }
        }
    }

    private void Climb(bool isOnGround)
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * (_climbSpeed * Time.deltaTime);
        }

        if (isOnGround)
        {
            transform.position += Vector3.back * (_climbSpeed * Time.deltaTime);
        }
        else
        {
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.down * (_climbSpeed * Time.deltaTime);
            }
        }
        
        
        
    }
    
    private void Init()
    {
        IsOnClimbed = false;
    }
}
