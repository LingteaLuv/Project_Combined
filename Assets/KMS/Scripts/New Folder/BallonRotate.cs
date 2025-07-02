using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonRotate : MonoBehaviour
{
    private Transform _camera;

    private void Start()
    {
        _camera = Camera.main.transform;
    }
    private void Update()
    {
        transform.forward = _camera.forward;
    }
}
