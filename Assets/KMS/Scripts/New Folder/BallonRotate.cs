using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallonRotate : MonoBehaviour
{
    private Transform _camera;

    [SerializeField] public Image N;
    [SerializeField] public Image A;
    [SerializeField] public Image C;

    private void Start()
    {
        _camera = Camera.main.transform;
    }
    private void Update()
    {
        transform.forward = _camera.forward;
    }

    public void SetAvailable()
    {
        N.enabled = false;
        A.enabled = true;
        C.enabled = false;
    }
    public void SetNone()
    {
        N.enabled = true;
        A.enabled = false;
        C.enabled = false;
    }
    public void SetCompleted()
    {
        N.enabled = false;
        A.enabled = false;
        C.enabled = true;
    }
}
