using Cinemachine;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private CinemachineVirtualCamera _virtualCamera;
    //[SerializeField] private Transform _lookatTarget;
    //[SerializeField] private Transform _fallowTarget;
    [SerializeField] public CinemachineBrain _cinemachineBrain;


    [Header("Mouse Config")]
    [SerializeField][Range(0, 5)] private float _mouseSensitivity = 1;

    public float Offset { get; private set; }
    
    private void Update()
    {
        Offset += Input.GetAxis("Mouse X") * _mouseSensitivity;
    }
    public void PauseCamera()
    {
        _cinemachineBrain.enabled = false;
    }

    // 카메라 재개
    public void ResumeCamera()
    {
        _cinemachineBrain.enabled = true;
    }

    // lookat, fallow 변경방식
    /*
    public void FocusOnLootable(Transform lootableTarget)
    {
        _virtualCamera.Follow = lootableTarget;
        _virtualCamera.LookAt = lootableTarget;
    }
    public void ResetFocusToPlayer()
    {
        _virtualCamera.Follow = _fallowTarget;
        _virtualCamera.LookAt = _lookatTarget;
    }*/
}