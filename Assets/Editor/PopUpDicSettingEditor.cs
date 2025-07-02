using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PopUpDictionary))]
public class PopUpDicSettingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PopUpDictionary popUpSetting = (PopUpDictionary)target;

        if (GUILayout.Button("Generate PopUpDictionary"))
        {
            popUpSetting.PopUpTextLoad();
            EditorUtility.SetDirty(popUpSetting); // 씬 저장 가능하게 표시
        }
    }
}

[CustomEditor(typeof(DialogueDictionary))]
public class DialogueDicSettingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogueDictionary dialogueSetting = (DialogueDictionary)target;

        if (GUILayout.Button("Generate DialogueDictionary"))
        {
            dialogueSetting.GenerateDic();
            EditorUtility.SetDirty(dialogueSetting); // 씬 저장 가능하게 표시
        }
    }
}