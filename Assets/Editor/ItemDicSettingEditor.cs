using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemDictionary))]

public class ItemDicSettingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ItemDictionary recipeSetting = (ItemDictionary)target;

        if (GUILayout.Button("Generate Dictionary"))
        {
            recipeSetting.GenerateDic();
            EditorUtility.SetDirty(recipeSetting); // 씬 저장 가능하게 표시
        }
    }
}
