using UnityEngine;

/// <summary>
/// LightingManager에서 사용하는 조명 관련 색상 데이터를 담은 ScriptableObject입니다.
/// 게임 내 시간의 정규화 비율(0 ~ 1)에 따라 Ambient, Directional, Fog 색상을 설정할 수 있습니다.
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset", menuName = "Environment/LightingPreset", order = 1)]
public class LightingPreset : ScriptableObject
{
    [Tooltip("Ambient Light 색상을 시간 비율에 따라 조절합니다.")]
    public Gradient AmbientColor;

    [Tooltip("Directional Light 색상을 시간 비율에 따라 조절합니다.")]
    public Gradient DirectionalColor;

    [Tooltip("Fog 색상을 시간 비율에 따라 조절합니다.")]
    public Gradient FogColor;
}