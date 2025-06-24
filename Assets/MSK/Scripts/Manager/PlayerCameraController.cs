using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _avatarTransform;
    [SerializeField] private Camera _mainCamera;

    [Header("Mouse Config")]
    [SerializeField][Range(-90, 0)] private float _mouseMinPitch;
    [SerializeField][Range(0, 90)] private float _mouseMaxPitch;
    [SerializeField][Range(0, 5)] private float _mouseSensitivity = 1;

    [Header("Camera Distance")]
    [SerializeField] private float _cameraDistance = 5f;
    [SerializeField] private float _cameraHeight = 1.5f;

    public float Offset { get; private set; }
    public float YPitch {  get; private set; }

    private void Update()
    {
        Vector2 mouseDelta = GetMouseDelta();
        Offset += mouseDelta.x * _mouseSensitivity;
        YPitch += mouseDelta.y * _mouseSensitivity;
        YPitch = Mathf.Clamp(YPitch, _mouseMinPitch, _mouseMaxPitch);

        Vector3 targetPosition = _avatarTransform.position + Vector3.up * _cameraHeight;

        Quaternion rotation = Quaternion.Euler(YPitch, Offset, 0f);
        Vector3 cameraOffset = rotation * new Vector3(0f, 0f, -_cameraDistance);

        _cameraTransform.position = targetPosition + cameraOffset;
        _cameraTransform.LookAt(targetPosition);
        Vector3 lookDirection = _cameraTransform.forward;
    }

    private Vector2 GetMouseDelta()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        return new Vector2(mouseX, mouseY);
    }
}
