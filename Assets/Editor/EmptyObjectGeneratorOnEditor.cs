using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EmptyObjectGenerator))]
public class EmptyObjectGeneratorOnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EmptyObjectGenerator generator = (EmptyObjectGenerator)target;

        if (GUILayout.Button("Generate EmptyObject"))
        {
            generator.Generate();
            EditorUtility.SetDirty(generator); // 씬 저장 가능하게 표시
        }
    }
}
