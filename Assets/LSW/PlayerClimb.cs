using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    [Header("Drag&Drop")] 
    [SerializeField] private float _climbSpeed;

    public void Climb(bool isOnGround)
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * (_climbSpeed * Time.deltaTime);
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (isOnGround)
            {
                transform.position -= transform.forward * (_climbSpeed * Time.deltaTime);
            }
            else
            {
                transform.position += Vector3.down * (_climbSpeed * Time.deltaTime);
            }
        }
    }
}
