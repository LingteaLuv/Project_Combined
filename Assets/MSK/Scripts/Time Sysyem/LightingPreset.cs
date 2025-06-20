using UnityEngine;

/// <summary>
/// LightingManager���� ����ϴ� ���� ���� ���� �����͸� ���� ScriptableObject�Դϴ�.
/// ���� �� �ð��� ����ȭ ����(0 ~ 1)�� ���� Ambient, Directional, Fog ������ ������ �� �ֽ��ϴ�.
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset", menuName = "Environment/LightingPreset", order = 1)]
public class LightingPreset : ScriptableObject
{
    [Tooltip("Ambient Light ������ �ð� ������ ���� �����մϴ�.")]
    public Gradient AmbientColor;

    [Tooltip("Directional Light ������ �ð� ������ ���� �����մϴ�.")]
    public Gradient DirectionalColor;

    [Tooltip("Fog ������ �ð� ������ ���� �����մϴ�.")]
    public Gradient FogColor;
}