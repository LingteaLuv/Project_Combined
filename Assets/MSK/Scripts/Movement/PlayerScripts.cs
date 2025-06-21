using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScripts : MonoBehaviour
{

    [Header("Player Movement")]
    public float movementSpeed = 4f;
    public CameraController cameraController;
    public float rotSpeed = 5f;

    private Quaternion requireRotation;

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float movementAmount = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

        var movementInput = (new Vector3(horizontal,0,vertical)).normalized;

        var movementDirection = (cameraController.FlatForward * movementInput.z + cameraController.FlatRight * movementInput.x).normalized;

        if (movementAmount > 0)
        {
            transform.position += movementDirection * movementSpeed * Time.deltaTime;
            requireRotation = Quaternion.LookRotation(movementDirection);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, requireRotation, rotSpeed * Time.deltaTime);
    }
}
