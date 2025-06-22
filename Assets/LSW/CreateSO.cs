using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateSO : EditorWindow
{
    private TextAsset csvFile;
    private TextAsset weaponCsvFile;

    [MenuItem("Tools/CSV to ScriptableObjects")]
    public static void ShowWindow()
    {
        GetWindow<CreateSO>("CSV to SO Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV To ScriptableObject Generator", EditorStyles.boldLabel);
        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV File", csvFile, typeof(TextAsset), false);
        weaponCsvFile = (TextAsset)EditorGUILayout.ObjectField("Weapon CSV", weaponCsvFile, typeof(TextAsset), false);
        
        if (GUILayout.Button("Generate ScriptableObjects"))
        {
            Debug.Log("진입1");
            if (csvFile != null)
            {
                Debug.Log("진입2");
                CreateSOFromCSV(csvFile);
            }
            else
            {
                Debug.LogWarning("CSV 파일이 지정되지 않았습니다.");
            }
        }
    }
    
    private void CreateSOFromCSV(TextAsset csvFile)
    {
        Dictionary<int, string[]> weaponData = new Dictionary<int, string[]>();

        if (weaponCsvFile != null)
        {
            Debug.Log("진입3");
            string[] weoponLines = weaponCsvFile.text.Split('\n');
            for (int i = 1; i < weoponLines.Length; i++)
            {
                string line = weoponLines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;
                string[] parts = line.Split(',');
                int id = int.Parse(parts[0]);
                weaponData[id] = parts;
            }
        }
        
        if (csvFile == null) return;
        string[] lines = csvFile.text.Split('\n');

        string folderPath = "Assets/ScriptableObjects/Items";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.Log("진입4");
            AssetDatabase.CreateFolder("Assets/ScriptableObjects","Items");
        }

        for (int i = 1; i < lines.Length; i++)
        {
            // 문장의 앞,뒤 공백 제거
            string line = lines[i].Trim();

            // 공백을 제거했을 때 아무 것도 없는 경우 스킵
            if (string.IsNullOrEmpty(line)) continue;

            // 문장을 ,(쉼표)로 구분
            string[] parts = line.Split(',');
            string itemId = parts[0];
            string name = parts[1];
            string description = parts[2];
            string itemType = parts[3];
            string icon = parts[4];

            if (itemType == ItemType.Weapon.ToString())
            {
                Debug.Log("진입5");
                WeaponItem_temp weaponItem = ScriptableObject.CreateInstance<WeaponItem_temp>();
                weaponItem.ItemId = int.Parse(itemId);
                weaponItem.Name = name;
                weaponItem.Description = description;
                weaponItem.ItemType = (ItemType)Enum.Parse(typeof(ItemType),itemType);
                //weaponItem.Icon = BringIcon(icon)

                int id = int.Parse(itemId);
                if (weaponData.ContainsKey(id))
                {
                    Debug.Log("진입6");
                    string[] weaponParts = weaponData[id];
                    weaponItem.Durability = int.Parse(weaponParts[1]);
                    weaponItem.AtkDamage = int.Parse(weaponParts[2]);
                }
                
                string assetPath = $"{folderPath}/Item_{itemId}_{name}.Asset";
                AssetDatabase.CreateAsset(weaponItem, assetPath);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
