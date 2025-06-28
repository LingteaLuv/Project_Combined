using Cinemachine;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private CinemachineVirtualCamera _virtualCamera;
    //[SerializeField] private Transform _lookatTarget;
    //[SerializeField] private Transform _fallowTarget;
    [SerializeField] public CinemachineBrain _cinemachineBrain;
    [SerializeField] private Transform _cameraRig;
    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;

    [Header("Mouse Config")]
    [SerializeField][Range(0, 1)] private float _mouseSensitivityX;

    [SerializeField] [Range(0, 1)] private float _mouseSensitivityY;

    public float OffsetX { get; private set; }
    public float OffsetY { get; private set; }
    
    private void Update()
    {
        OffsetX += Input.GetAxis("Mouse X") * _mouseSensitivityX;
        OffsetY += Input.GetAxis("Mouse Y") * _mouseSensitivityY;
        OffsetY = Mathf.Clamp(OffsetY, _minPitch, _maxPitch); 
        _cameraRig.localEulerAngles = new Vector3(OffsetY, 0f, 0f);
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