using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Mouse Config")]
    [SerializeField][Range(0, 5)] private float _mouseSensitivity = 1;

    public float Offset { get; private set; } // Yaw (X축 회전)

    private void Update()
    {
        Offset += Input.GetAxis("Mouse X") * _mouseSensitivity;
    }
}