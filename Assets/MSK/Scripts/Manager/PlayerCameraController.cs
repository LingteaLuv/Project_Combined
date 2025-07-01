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
    [SerializeField] private CinemachineVirtualCamera _playerCam;

    [Header("Mouse Config")]
    [SerializeField][Range(0, 1)] public Property<float> MouseXSensitivity;

    [SerializeField] [Range(0, 1)] public Property<float> MouseYSensitivity;

    public float OffsetX { get; private set; }
    public float OffsetY { get; private set; }
    public bool CameraMove { get; private set; }

    private void Awake()
    {
        CameraMove = true;
        Init();
    }
    
    private void Start()
    {
        // 설정 값으로 초기화
        FOVUpdate(SettingManager.Instance.FOV.Value);
        MouseXSpeedUpdate(SettingManager.Instance.MouseXSpeed.Value);
        MouseYSpeedUpdate(SettingManager.Instance.MouseYSpeed.Value);
        
        // Property 이벤트 메서드 구독
        SettingManager.Instance.FOV.OnChanged += FOVUpdate;
        SettingManager.Instance.MouseXSpeed.OnChanged += MouseXSpeedUpdate;
        SettingManager.Instance.MouseYSpeed.OnChanged += MouseYSpeedUpdate;

        GameManager.Instance.OnGameOver += Unsubscribe;
    }
    
    private void Unsubscribe()
    {
        // Destroy될 경우 구독 해제
        SettingManager.Instance.FOV.OnChanged -= FOVUpdate;
        SettingManager.Instance.MouseXSpeed.OnChanged -= MouseXSpeedUpdate;
        SettingManager.Instance.MouseYSpeed.OnChanged -= MouseYSpeedUpdate;
    }
    
    // 시야각 설정 값 반영
    private void FOVUpdate(float value)
    {
        float fov = Mathf.Lerp(50, 70, value);
        _playerCam.m_Lens.FieldOfView = fov;
    }
    
    // 마우스 민감도 설정 값 반영
    private void MouseXSpeedUpdate(float value)
    {
        float speedX = Mathf.Lerp(0.1f, 2f, value);
        MouseXSensitivity.Value = speedX;
    }
    
    private void MouseYSpeedUpdate(float value)
    {
        float speedY = Mathf.Lerp(0.1f, 2f, value);
        MouseYSensitivity.Value = speedY;
    }
    
    private void Update()
    {
        if (!CameraMove)
            return;
        OffsetX += Input.GetAxis("Mouse X") * MouseXSensitivity.Value;
        OffsetY += Input.GetAxis("Mouse Y") * MouseYSensitivity.Value;
        OffsetY = Mathf.Clamp(OffsetY, _minPitch, _maxPitch); 
        _cameraRig.localEulerAngles = new Vector3(OffsetY, 0f, 0f);
    }

    public void PauseCamera()
    {
        CameraMove = !CameraMove;
    }

    // TODO : 카메라 재개
    public void ResumeCamera()
    {
    }

    private void Init()
    {
        MouseXSensitivity = new Property<float>(0.5f);
        MouseYSensitivity = new Property<float>(0.5f);
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