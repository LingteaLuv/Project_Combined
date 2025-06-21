using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    public float rotationY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        var state = vCam.State;

        var rotation = state.FinalOrientation;

        var euler = rotation.eulerAngles;

        rotationY = euler.y;

        var roundedRotationY = Mathf.RoundToInt(rotationY);
    
    }
    public Quaternion flatRotation => Quaternion.Euler(0, rotationY, 0);
}

