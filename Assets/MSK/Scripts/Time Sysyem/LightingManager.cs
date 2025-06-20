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
    [SerializeField] private Light _directionalLight;
    [SerializeField] private LightingPreset _preset;
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
        if (_preset == null || _timeManager == null)
            return;

        float hour = _timeManager.CurrentHour.Value;
        float minute = _timeManager.CurrentMinute.Value;
        float normalizedTime = (hour + minute / 60f) / 24f;

        UpdateLighting(normalizedTime);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// 현재 시간 비율에 따라 환경 조명 값(Ambient, Fog, DirectionalLight)을 조정합니다.
    /// </summary>
    /// <param name="timePercent">하루 시간의 정규화 비율 (0.0 ~ 1.0)</param>
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = _preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = _preset.FogColor.Evaluate(timePercent);

        if (_directionalLight != null)
        {
            _directionalLight.color = _preset.DirectionalColor.Evaluate(timePercent);
            _directionalLight.transform.localRotation =
                Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
        }
    }

    /// <summary>
    /// 인스펙터에서 Directional Light가 비어 있는 경우 자동으로 씬 내에서 탐색합니다.
    /// </summary>
    private void OnValidate()
    {
        if (_directionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            _directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    _directionalLight = light;
                    return;
                }
            }
        }
    }
    #endregion
}