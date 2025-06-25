using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RecipeSetting))]

public class ItemDicSettingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RecipeSetting recipeSetting = (RecipeSetting)target;

        if (GUILayout.Button("Set Recipe Values"))
        {
            recipeSetting.SetValueEditor();
            EditorUtility.SetDirty(recipeSetting); // 씬 저장 가능하게 표시
        }
    }
}
