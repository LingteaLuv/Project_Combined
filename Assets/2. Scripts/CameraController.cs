using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Drag&Drop")] [SerializeField] private GameObject _player;
    [SerializeField] private float _rotateSpeed;

    private Vector3 _offset;

    private float _curRotationX;
    private float _curRotationY;
    public Vector3 FlatForward => new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
    public Vector3 FlatRight => new Vector3(transform.right.x, 0f, transform.right.z).normalized;
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        SetCameraPosition();
        SetCameraRotation();
    }

    private void SetCameraPosition()
    {
        Vector3 backOffset = _player.transform.TransformDirection(_offset);
        Vector3 desiredPos = _player.transform.position + backOffset;
        transform.position = desiredPos;
    }

    private void SetCameraRotation()
    {
        transform.LookAt(_player.transform.position + _player.transform.forward * 10f);
    }
    
    private void Init()
    {
        _offset = new Vector3(3, 3, -4);
    }
}
