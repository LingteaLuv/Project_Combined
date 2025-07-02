using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Slider _lightSlider;
    [SerializeField] private Slider _fovSlider;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _mouseXSlider;
    [SerializeField] private Slider _mouseYSlider;

    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _exitButton;

    private float _cacheBrightness;
    private float _cacheFOV;
    private float _cacheSound;
    private float _cacheMouseXSpeed;
    private float _cacheMouseYSpeed;


    private void Start()
    {
        _lightSlider.onValueChanged.AddListener((value) => SettingManager.Instance.SetBrightness(value));
        _fovSlider.onValueChanged.AddListener((value) => SettingManager.Instance.SetFOV(value));
        _soundSlider.onValueChanged.AddListener((value) => SettingManager.Instance.SetSound(value));
        _mouseXSlider.onValueChanged.AddListener((value) => SettingManager.Instance.SetMouseXSpeed(value));
        _mouseYSlider.onValueChanged.AddListener((value) => SettingManager.Instance.SetMouseYSpeed(value));

        _saveButton.onClick.AddListener(() => SettingSave());
        _exitButton.onClick.AddListener(() =>
        {
            SettingUpdate();
            gameObject.SetActive(false);
        });

        CacheInit();
        SettingUpdate();
        gameObject.SetActive(false);
    }

    private void CacheInit()
    {
        _cacheBrightness = SettingManager.Instance.Brightness.Value;
        _cacheFOV = SettingManager.Instance.FOV.Value;
        _cacheSound = SettingManager.Instance.Sound.Value;
        _cacheMouseXSpeed = SettingManager.Instance.MouseXSpeed.Value;
        _cacheMouseYSpeed = SettingManager.Instance.MouseYSpeed.Value;
    }

    private void SettingUpdate()
    {
        SettingManager.Instance.SetBrightness(_cacheBrightness);
        SettingManager.Instance.SetFOV(_cacheFOV);
        SettingManager.Instance.SetSound(_cacheSound);
        SettingManager.Instance.SetMouseXSpeed(_cacheMouseXSpeed);
        SettingManager.Instance.SetMouseYSpeed(_cacheMouseYSpeed);

        _lightSlider.value = _cacheBrightness;
        _fovSlider.value = _cacheFOV;
        _soundSlider.value = _cacheSound;
        _mouseXSlider.value = _cacheMouseXSpeed;
        _mouseYSlider.value = _cacheMouseYSpeed;
    }

    private void SettingSave()
    {
        _cacheBrightness = _lightSlider.value;
        _cacheFOV = _fovSlider.value;
        _cacheSound = _soundSlider.value;
        _cacheMouseXSpeed = _mouseXSlider.value;
        _cacheMouseYSpeed = _mouseYSlider.value;
    }
}
