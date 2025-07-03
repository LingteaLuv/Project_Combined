using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MonsterInfoSetting : EditorWindow
{
    private TextAsset _monsterFile;
    private TextAsset _playerFile;

    [MenuItem("Tools/CSV to Character")]
    public static void ShowWindow()
    {
        GetWindow<MonsterInfoSetting>("CSV to Character");
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV To Character", EditorStyles.boldLabel);
        _monsterFile = (TextAsset)EditorGUILayout.ObjectField("Monster CSV", _monsterFile, typeof(TextAsset), false);
        _playerFile = (TextAsset)EditorGUILayout.ObjectField("Player CSV", _playerFile, typeof(TextAsset), false);

        if (GUILayout.Button("Generate Monster"))
        {
            if (_monsterFile != null)
            {
                CreateMonsterFromCSV();
            }
            else
            {
                Debug.LogWarning("CSV 파일이 지정되지 않았습니다.");
            }
        }

        if (GUILayout.Button("Generate Player"))
        {
            if (_playerFile != null)
            {
                CreatePlayerFromCSV();
            }
            else
            {
                Debug.LogWarning("CSV 파일이 지정되지 않았습니다.");
            }
        }
    }

    private void CreateMonsterFromCSV()
    {
        if (_monsterFile == null) return;
        string[] lines = _monsterFile.text.Split('\n');

        string folderPath = "Assets/ScriptableObjects/Character";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/ScriptableObjects","Character");
        }

        for (int i = 1; i < lines.Length; i++)
        {
            // 문장의 앞,뒤 공백 제거
            string line = lines[i].Trim();

            // 공백을 제거했을 때 아무 것도 없는 경우 스킵
            if (string.IsNullOrEmpty(line)) continue;

            // 문장을 ,(쉼표)로 구분
            string[] parts = line.Split(',');
            
            MonsterInfo monster = ScriptableObject.CreateInstance<MonsterInfo>();

            string monsterID = parts[0];
            monster.EnemyID = int.Parse(parts[0]);
            monster.Name = parts[1];
            monster.MaxHP = float.Parse(parts[2]);
            monster.AtkType = parts[3];
            monster.AtkDamage = int.Parse(parts[4]);
            monster.CastTime = float.Parse(parts[5]);
            monster.RecoveryFrame = float.Parse(parts[6]);
            monster.AtkCoolDown = float.Parse(parts[7]);
            monster.AtkSpeed = float.Parse(parts[8]);
            monster.AtkRange= float.Parse(parts[9]);
            monster.MoveSpeed = float.Parse(parts[10]);
            monster.ChaseMoveSpeed = float.Parse(parts[11]);
            monster.NightMoveSpeed = float.Parse(parts[12]);
            monster.SightRange = float.Parse(parts[13]);
            monster.SightAngle = float.Parse(parts[14]);
            monster.HearingRange = float.Parse(parts[15]);
            monster.PatrolSight = float.Parse(parts[16]);
            monster.ChaseSight = float.Parse(parts[17]);
            monster.DeactivateHearing = float.Parse(parts[18]);
            monster.PatrolRadius = float.Parse(parts[19]);
            monster.SearchTime = float.Parse(parts[20]);
            monster.LootChance = float.Parse(parts[21]);
            monster.EnemyLootID = parts[22];
            monster.EnemyLootGridChanceID = parts[23];
            
            string assetPath = $"{folderPath}/Quest_{monsterID}.Asset";
            AssetDatabase.CreateAsset(monster, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private void CreatePlayerFromCSV()
    {
        if (_playerFile == null) return;
        string[] lines = _playerFile.text.Split('\n');

        string folderPath = "Assets/ScriptableObjects/Character";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/ScriptableObjects","Character");
        }

        for (int i = 1; i < lines.Length; i++)
        {
            // 문장의 앞,뒤 공백 제거
            string line = lines[i].Trim();

            // 공백을 제거했을 때 아무 것도 없는 경우 스킵
            if (string.IsNullOrEmpty(line)) continue;

            // 문장을 ,(쉼표)로 구분
            string[] parts = line.Split(',');
            
            MonsterInfo monster = ScriptableObject.CreateInstance<MonsterInfo>();

            string monsterID = parts[0];
            monster.EnemyID = int.Parse(parts[0]);
            monster.Name = parts[1];
            monster.MaxHP = float.Parse(parts[2]);
            monster.AtkType = parts[3];
            monster.AtkDamage = int.Parse(parts[4]);
            monster.CastTime = float.Parse(parts[5]);
            monster.RecoveryFrame = float.Parse(parts[6]);
            monster.AtkCoolDown = float.Parse(parts[7]);
            monster.AtkSpeed = float.Parse(parts[8]);
            monster.AtkRange= float.Parse(parts[9]);
            monster.MoveSpeed = float.Parse(parts[10]);
            monster.ChaseMoveSpeed = float.Parse(parts[11]);
            monster.NightMoveSpeed = float.Parse(parts[12]);
            monster.SightRange = float.Parse(parts[13]);
            monster.SightAngle = float.Parse(parts[14]);
            monster.HearingRange = float.Parse(parts[15]);
            monster.PatrolSight = float.Parse(parts[16]);
            monster.ChaseSight = float.Parse(parts[17]);
            monster.DeactivateHearing = float.Parse(parts[18]);
            monster.PatrolRadius = float.Parse(parts[19]);
            monster.SearchTime = float.Parse(parts[20]);
            monster.LootChance = float.Parse(parts[21]);
            monster.EnemyLootID = parts[22];
            monster.EnemyLootGridChanceID = parts[23];
            
            string assetPath = $"{folderPath}/Quest_{monsterID}.Asset";
            AssetDatabase.CreateAsset(monster, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
