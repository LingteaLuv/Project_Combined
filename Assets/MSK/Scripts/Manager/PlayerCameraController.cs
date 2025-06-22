using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _avatarTransform;

    [Header("Mouse Config")]
    [SerializeField][Range(-90, 0)] private float _mouseMinPitch;
    [SerializeField][Range(0, 90)] private float _mouseMaxPitch;
    [SerializeField][Range(0, 5)] private float _mouseSensitivity = 1;

    private Vector2 _currentRotation;
    public Vector2 CurrentRotation => _currentRotation;

    private void Update()
    {
        Vector2 mouseDelta = GetMouseDelta();

        _currentRotation.x += mouseDelta.x * _mouseSensitivity;
        _currentRotation.y = Mathf.Clamp(
            _currentRotation.y + mouseDelta.y * _mouseSensitivity,
            _mouseMinPitch,
            _mouseMaxPitch
        );
    }

    private Vector2 GetMouseDelta()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        return new Vector2(mouseX, mouseY);
    }
}
