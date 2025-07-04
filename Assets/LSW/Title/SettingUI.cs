using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Slider _fovSlider;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _mouseXSlider;
    [SerializeField] private Slider _mouseYSlider;

    [SerializeField] private TextMeshProUGUI _fovText;
    [SerializeField] private TextMeshProUGUI _bgmText;
    [SerializeField] private TextMeshProUGUI _sfxText;
    [SerializeField] private TextMeshProUGUI _mouseXText;
    [SerializeField] private TextMeshProUGUI _mouseYText;
    
    [SerializeField] private Button _exitButton;

    [SerializeField] private Toggle _fullScreenToggle;
    [SerializeField] private Toggle _windowToggle;
    
    private float _cacheFOV;
    private float _cacheBgm;
    private float _cacheSfx;
    private float _cacheMouseXSpeed;
    private float _cacheMouseYSpeed;
    private bool _cacheFullToggle;
    private bool _cacheWindowToggle;


    private void Start()
    {
        Debug.Log("FOV (Init): " + SettingManager.Instance.FOV.Value);
        Debug.Log("FOV Slider Value (Before): " + _fovSlider.value);
        
        _fovSlider.onValueChanged.AddListener((value) =>
        {
            Debug.Log("FOV Changed To: " + value);
            _fovText.text = $"{(int)(value * 100)}";
            SettingManager.Instance.SetFOV(value);
        });
        _bgmSlider.onValueChanged.AddListener((value) =>
        {
            _bgmText.text = $"{(int)(value * 100)}";
            SettingManager.Instance.SetBGMSound(value);
        });
        _sfxSlider.onValueChanged.AddListener((value) =>
        {
            _sfxText.text = $"{(int)(value * 100)}";
            SettingManager.Instance.SetSFXSound(value);
        });
        _mouseXSlider.onValueChanged.AddListener((value) =>
        {
            _mouseXText.text = $"{(int)(value * 100)}";
            SettingManager.Instance.SetMouseXSpeed(value);
        });
        _mouseYSlider.onValueChanged.AddListener((value) =>
        {
            _mouseYText.text = $"{(int)(value * 100)}";
            SettingManager.Instance.SetMouseYSpeed(value);
        });

        _fullScreenToggle.onValueChanged.AddListener((value) => SettingManager.Instance.SetFullScreen(value));
        _windowToggle.onValueChanged.AddListener((value) => SettingManager.Instance.SetWindow(value));
        
        _exitButton.onClick.AddListener(() =>
        {
            SettingSave();
            SettingUpdate();
            gameObject.SetActive(false);
        });

        CacheInit();
        SettingUpdate();
        gameObject.SetActive(false);
    }

    private void CacheInit()
    {
        _cacheFOV = SettingManager.Instance.FOV.Value;
        _cacheBgm = SettingManager.Instance.BGMSound.Value;
        _cacheSfx = SettingManager.Instance.SFXSound.Value;
        _cacheMouseXSpeed = SettingManager.Instance.MouseXSpeed.Value;
        _cacheMouseYSpeed = SettingManager.Instance.MouseYSpeed.Value;
        _cacheFullToggle = SettingManager.Instance.IsFullScreen.Value;
        _cacheWindowToggle = SettingManager.Instance.IsWindow.Value;
    }

    private void SettingUpdate()
    {
        SettingManager.Instance.SetFOV(_cacheFOV);
        SettingManager.Instance.SetBGMSound(_cacheBgm);
        SettingManager.Instance.SetSFXSound(_cacheSfx);
        SettingManager.Instance.SetMouseXSpeed(_cacheMouseXSpeed);
        SettingManager.Instance.SetMouseYSpeed(_cacheMouseYSpeed);
        SettingManager.Instance.SetFullScreen(_cacheFullToggle);
        SettingManager.Instance.SetWindow(_cacheWindowToggle);
        
        _fovSlider.value = _cacheFOV;
        _bgmSlider.value = _cacheBgm;
        _sfxSlider.value = _cacheSfx;
        _mouseXSlider.value = _cacheMouseXSpeed;
        _mouseYSlider.value = _cacheMouseYSpeed;
        _fullScreenToggle.isOn = _cacheFullToggle;
        _windowToggle.isOn = _cacheWindowToggle;
    }

    private void SettingSave()
    {
        _cacheFOV = _fovSlider.value;
        _cacheBgm = _bgmSlider.value;
        _cacheSfx = _sfxSlider.value;
        _cacheMouseXSpeed = _mouseXSlider.value;
        _cacheMouseYSpeed = _mouseYSlider.value;
        _cacheFullToggle = _fullScreenToggle.isOn;
        _cacheWindowToggle = _windowToggle.isOn;
    }
}
