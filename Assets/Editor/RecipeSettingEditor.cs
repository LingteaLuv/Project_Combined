using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(RecipeSetting))]
public class RecipeSettingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RecipeSetting recipeSetting = (RecipeSetting)target;

        if (GUILayout.Button("Set Recipe Values"))
        {
            recipeSetting.SetValueEditor();
            EditorUtility.SetDirty(recipeSetting);
            PrefabUtility.RecordPrefabInstancePropertyModifications(recipeSetting);
            EditorSceneManager.MarkSceneDirty(recipeSetting.gameObject.scene);
        }
    }
}
