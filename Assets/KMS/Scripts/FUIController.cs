using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FUIController : MonoBehaviour
{
    [SerializeField] private Transform _f;
    private Transform _camera;
    public CanvasGroup CanvasGroup;

    private void Start()
    {
        _camera = Camera.main.transform;
    }
    private void Update()
    {
        _f.forward = _camera.forward;
    }
}
