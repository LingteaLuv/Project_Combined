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
    /// ���� �ð� ������ ���� ȯ�� ���� ��(Ambient, Fog, DirectionalLight)�� �����մϴ�.
    /// </summary>
    /// <param name="timePercent">�Ϸ� �ð��� ����ȭ ���� (0.0 ~ 1.0)</param>
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
    /// �ν����Ϳ��� Directional Light�� ��� �ִ� ��� �ڵ����� �� ������ Ž���մϴ�.
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