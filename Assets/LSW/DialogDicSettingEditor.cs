using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogDictionary))]
public class DialogDicSettingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogDictionary dialogSetting = (DialogDictionary)target;

        if (GUILayout.Button("Generate Dictionary"))
        {
            dialogSetting.PopUpTextLoad();
            EditorUtility.SetDirty(dialogSetting); // 씬 저장 가능하게 표시
        }
    }
}