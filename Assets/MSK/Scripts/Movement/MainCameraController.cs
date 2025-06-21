using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;

public class MainCameraController : MonoBehaviour
{
   // public CinemachineVirtualCamera virCam;
    public float rotationY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {/*
        var state = virCam.State;

        var rotation = state.FinalOrientation;

        var euler = rotation.eulerAngles;

        rotationY = euler.y;

        var roundedRotationY = Mathf.RoundToInt(rotationY);
        */
    }
    public Quaternion floatRotation => Quaternion.Euler(0, rotationY, 0);
}

