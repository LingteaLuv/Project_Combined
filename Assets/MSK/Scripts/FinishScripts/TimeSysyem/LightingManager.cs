using System.Collections;
using UnityEngine;

/// <summary>
/// TimeManager의 현재 시간 정보를 기반으로 조명 및 환경 설정을 동적으로 조절합니다.
/// 낮과 밤의 시간대에 따라 Ambient Light, Fog Color, Directional Light의 색상과 회전값을 실시간으로 변경합니다.
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
    /// 현재 시간 비율에 따라 환경 조명 값(Ambient, Fog, DirectionalLight)을 조정합니다.
    /// </summary>
    /// <param name="timePercent">하루 시간의 정규화 비율 (0.0 ~ 1.0)</param>
    private void UpdateLighting(float timePercent)
    {
        bool isDay = _timeManager.CurrentTimeOfDay.Value == DayTime.Morning
                  || _timeManager.CurrentTimeOfDay.Value == DayTime.Day;

        // 현재 시간대에 맞는 Preset 선택
        LightingPreset activePreset = isDay ? _dayPreset : _nightPreset;
        if (activePreset == null) return;

        // 환경광 및 안개색 설정
        RenderSettings.ambientLight = activePreset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = activePreset.FogColor.Evaluate(timePercent);

        float angle = (timePercent * 360f) - 90f;
        // 광원 설정
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
    /// 낮/밤 상태에 따라 태양광(_dayLight)과 달빛(_moonLight)을 전환합니다.
    /// </summary>
    private void UpdateLightSwitch()
    {
        if (_timeManager == null) return;

        bool isDay = _timeManager.CurrentTimeOfDay.Value == DayTime.Morning
                  || _timeManager.CurrentTimeOfDay.Value == DayTime.Day;
        if (_dayLight != null)
            _dayLight.enabled = isDay;

        if (_moonLight != null)
            _moonLight.enabled = !isDay;
    }

    /// <summary>
    /// 인스펙터에서 Light가 비어 있는 경우 자동으로 씬 내에서 탐색합니다.
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
