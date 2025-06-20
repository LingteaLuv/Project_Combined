using System.Collections;
using UnityEngine;

/// <summary>
/// TimeManager�� ���� �ð� ������ ������� ���� �� ȯ�� ������ �������� �����մϴ�.
/// ���� ���� �ð��뿡 ���� Ambient Light, Fog Color, Directional Light�� ����� ȸ������ �ǽð����� �����մϴ�.
/// </summary>
[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Light _dayLight;
    [SerializeField] private Light _moonLight;
    [SerializeField] private LightingPreset _dayPreset;
    [SerializeField] private LightingPreset _nightPreset;
    #endregion

    #region Private Fields
    private TimeManager _timeManager;
    #endregion

    #region Unity MonoBehaviour
    private void Start()
    {
        _timeManager = TimeManager.Instance;
    }

    private void Update()
    {
        if (_timeManager == null)
            return;

        float hour = _timeManager.CurrentHour.Value;
        float minute = _timeManager.CurrentMinute.Value;
        float normalizedTime = (hour + minute / 60f) / 24f;

        UpdateLighting(normalizedTime);
        UpdateLightSwitch();
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// ���� �ð� ������ ���� ȯ�� ���� ��(Ambient, Fog, DirectionalLight)�� �����մϴ�.
    /// </summary>
    /// <param name="timePercent">�Ϸ� �ð��� ����ȭ ���� (0.0 ~ 1.0)</param>
    private void UpdateLighting(float timePercent)
    {
        bool isDay = _timeManager.CurrentTimeOfDay.Value == DayTime.Day;

        // ���� �ð��뿡 �´� Preset ����
        LightingPreset activePreset = isDay ? _dayPreset : _nightPreset;
        if (activePreset == null) return;

        // ȯ�汤 �� �Ȱ��� ����
        RenderSettings.ambientLight = activePreset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = activePreset.FogColor.Evaluate(timePercent);

        float angle = (timePercent * 360f) - 90f;
        // ���� ����
        if (_dayLight != null)
        {
            _dayLight.color = _dayPreset.DirectionalColor.Evaluate(timePercent);
            _dayLight.transform.localRotation = Quaternion.Euler(new Vector3(angle, 170f, 0f));
        }

        if (_moonLight != null)
        {
            _moonLight.color = _nightPreset.DirectionalColor.Evaluate(timePercent);
            _moonLight.transform.localRotation = Quaternion.Euler(new Vector3(angle + 180f, 170f, 0f));
        }
    }

    /// <summary>
    /// ��/�� ���¿� ���� �¾籤(_dayLight)�� �޺�(_moonLight)�� ��ȯ�մϴ�.
    /// </summary>
    private void UpdateLightSwitch()
    {
        if (_timeManager == null) return;

        bool isDay = _timeManager.CurrentTimeOfDay.Value == DayTime.Day;

        if (_dayLight != null)
            _dayLight.enabled = isDay;

        if (_moonLight != null)
            _moonLight.enabled = !isDay;
    }

    /// <summary>
    /// �ν����Ϳ��� Light�� ��� �ִ� ��� �ڵ����� �� ������ Ž���մϴ�.
    /// </summary>
    private void OnValidate()
    {
        if (_dayLight == null && RenderSettings.sun != null)
        {
            _dayLight = RenderSettings.sun;
        }
        else if (_dayLight == null)
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    _dayLight = light;
                    return;
                }
            }
        }
    }   
    #endregion
}
