using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LootDictionary))]
public class LootDicSettingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LootDictionary lootDicSetting = (LootDictionary)target;

        if (GUILayout.Button("Generate Dictionary"))
        {
            lootDicSetting.GenerateDic();
            EditorUtility.SetDirty(lootDicSetting); // 씬 저장 가능하게 표시
        }
    }
}
