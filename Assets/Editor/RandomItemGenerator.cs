using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(RandomLootTable))]
public class RandomItemGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RandomLootTable randomItem = (RandomLootTable)target;

        if (GUILayout.Button("Generate RandomItem"))
        {
            randomItem.GenerateItem();
            EditorUtility.SetDirty(randomItem); // 씬 저장 가능하게 표시
        }
    }
}
