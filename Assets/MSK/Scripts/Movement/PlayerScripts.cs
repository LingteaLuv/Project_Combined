using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScripts : MonoBehaviour
{

    [Header("Player Movement")]
    public float movementSpeed = 4f;
    public MainCameraController cameraController;
    public float rotSpeed = 5f;

    private Quaternion requireRotation;

    [Header("Player Movement")]
    public Animator animator;
    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        var movementInput = (new Vector3(horizontal,0,vertical)).normalized;

        var movementDirection = cameraController.flatRotation * movementInput;

        if (movementAmount > 0)
        {
            transform.position += movementDirection * movementSpeed * Time.deltaTime;
            requireRotation = Quaternion.LookRotation(movementDirection);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, requireRotation, rotSpeed * Time.deltaTime);
        animator.SetFloat("movementValue", movementAmount);
    }
}
