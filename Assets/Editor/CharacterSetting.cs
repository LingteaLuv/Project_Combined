using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterSetting : EditorWindow
{
    private TextAsset _monsterFile;
    private TextAsset _playerFile;

    [MenuItem("Tools/CSV to Character")]
    public static void ShowWindow()
    {
        GetWindow<CharacterSetting>("CSV to Character");
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
            monster.AtkDamage = int.Parse(parts[3]);
            monster.AtkCoolDown = float.Parse(parts[4]);
            monster.AtkRange= float.Parse(parts[5]);
            monster.MoveSpeed = float.Parse(parts[6]);
            monster.ChaseMoveSpeed = float.Parse(parts[7]);
            monster.NightMoveSpeed = float.Parse(parts[8]);
            monster.NightChaseMoveSpeed = float.Parse(parts[9]);
            monster.NightAtkCoolDown = float.Parse(parts[10]);
            monster.SightRange = float.Parse(parts[11]);
            monster.SightAngle = float.Parse(parts[12]);
            monster.HearingRange = float.Parse(parts[13]);
            monster.DeactivateHearing = float.Parse(parts[14]);
            monster.PatrolSight = float.Parse(parts[15]);
            monster.ChaseSight = float.Parse(parts[16]); 
            monster.PatrolRadius = float.Parse(parts[17]);
            monster.SearchTime = float.Parse(parts[18]);
            Debug.Log(parts[19]);
            monster.EnemyLootID = parts[19];
            monster.EnemyLootGridChanceID = parts[20];
            monster.IdleSfx1 = FileFinder.FindSFXByName(parts[21]);
            monster.IdleSfx2 = FileFinder.FindSFXByName(parts[22]);
            monster.IdleSfxRange = float.Parse(parts[23]);
            monster.ChaseSfx1 = FileFinder.FindSFXByName(parts[24]);
            monster.ChaseSfx2 = FileFinder.FindSFXByName(parts[25]);
            monster.AtkSfx = FileFinder.FindSFXByName(parts[26]);
            monster.HitSfx = FileFinder.FindSFXByName(parts[27]);
            monster.DieSfx = FileFinder.FindSFXByName(parts[28]);
            
            string assetPath = $"{folderPath}/Monster_{monsterID}.Asset";
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
            
            PlayerInfo player = ScriptableObject.CreateInstance<PlayerInfo>();

            string playerID = parts[0];
            player.PlayerID = int.Parse(parts[0]);
            player.MaxHP = float.Parse(parts[1]);
            player.MaxStamaina = float.Parse(parts[2]);
            player.MaxMoisture = float.Parse(parts[3]);
            player.StaminaRegen = int.Parse(parts[4]);
            player.StaminaCostRun = float.Parse(parts[5]);
            player.StaminaCostJump = float.Parse(parts[6]);
            player.StaminaCostMelee = float.Parse(parts[7]);
            player.SafeFallDistance = float.Parse(parts[8]);
            player.DeadFallDistance = float.Parse(parts[9]);
            player.FallDamage = float.Parse(parts[10]);
            player.HungerDecrease = float.Parse(parts[11]);
            player.HungerBuffThreshold = int.Parse(parts[12]);
            player.HungerBuffAtkSpeed = float.Parse(parts[13]);
            player.HungerDebuffThreshold = int.Parse(parts[14]);
            player.HungerDebuffAtkSpeed = float.Parse(parts[15]);
            player.MoistureDecrease = float.Parse(parts[16]);
            player.MoistureBuffThreshold = int.Parse(parts[17]);
            player.MoistureBuffMoveSpeed = float.Parse(parts[18]);
            player.MoistureDebuffThreshold = int.Parse(parts[19]);
            player.MoistureDebuffMoveSpeed = float.Parse(parts[20]);
            player.DepletionHp = float.Parse(parts[21]);
            player.HPRegen = float.Parse(parts[22]);
            player.HungerAndMoisture = float.Parse(parts[23]);
            player.MoveSpeed = float.Parse(parts[24]);
            player.MoveNoise = float.Parse(parts[25]);
            player.RunSpeed = float.Parse(parts[26]);
            player.RunNoise = float.Parse(parts[27]);
            player.CrouchSpeed = float.Parse(parts[28]);
            player.CrouchNoise = float.Parse(parts[29]);
            player.RunSFX = FileFinder.FindSFXByName(parts[30]);
            player.StaminaDepletionSFX = FileFinder.FindSFXByName(parts[31]);
            player.AtkSFX = FileFinder.FindSFXByName(parts[32]);
            player.AtkSFXCooldown = int.Parse(parts[33]);
            player.JumpSFX = FileFinder.FindSFXByName(parts[34]);
            player.HitSFX = FileFinder.FindSFXByName(parts[35]);
            player.HitSFXCooldown = int.Parse(parts[36]);
            player.DestroyEquipmentSFX = FileFinder.FindSFXByName(parts[37]);
            
            string assetPath = $"{folderPath}/Player_{playerID}.Asset";
            AssetDatabase.CreateAsset(player, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
