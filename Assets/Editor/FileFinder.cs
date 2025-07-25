#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class FileFinder
{
    /// <summary>
    /// 지정한 이름의 Sprite를 Resources가 아닌 AssetDatabase를 통해 찾아 반환합니다.
    /// </summary>
    public static Sprite FindSpriteByName(string name)
    {
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { "Assets/Imports/Sprites" }); 
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

            if (sprite != null && sprite.name == name)
            {
                return sprite;
            }
        }

        Debug.LogWarning($"Sprite with name \"{name}\" not found.");
        return null;
    }
    public static AudioClip FindSFXByName(string name)
    {
        if (name == "") return null;
        string[] guids = AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets/Imports/SFX" }); 
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);

            if (audioClip != null && audioClip.name == name)
            {
                return audioClip;
            }
        }

        Debug.LogWarning($"AudioClip with name \"{name}\" not found.");
        return null;
    }
}
#endif

