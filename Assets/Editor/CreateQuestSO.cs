using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateQuestSO : EditorWindow
{
    private TextAsset _questFile;
    private TextAsset _choiceFile;
    private TextAsset _dialogueFile;
    private TextAsset _npcFile;

    [MenuItem("Tools/CSV to Quest/Dialogue/NPC")]
    public static void ShowWindow()
    {
        GetWindow<CreateQuestSO>("CSV to Quest/Dialogue/NPC");
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV To Quest Generator", EditorStyles.boldLabel);
        _questFile = (TextAsset)EditorGUILayout.ObjectField("Quest CSV", _questFile, typeof(TextAsset), false);
        _choiceFile = (TextAsset)EditorGUILayout.ObjectField("Choice CSV", _choiceFile, typeof(TextAsset), false);
        _dialogueFile = (TextAsset)EditorGUILayout.ObjectField("Dialogue CSV", _dialogueFile, typeof(TextAsset), false);
        _npcFile = (TextAsset)EditorGUILayout.ObjectField("NPC CSV", _npcFile, typeof(TextAsset), false);

        if (GUILayout.Button("Generate Quest"))
        {
            if (_questFile != null)
            {
                CreateQuestFromCSV();
            }
            else
            {
                Debug.LogWarning("CSV 파일이 지정되지 않았습니다.");
            }
        }
        if (GUILayout.Button("Generate Choice"))
        {
            if (_choiceFile != null)
            {
                CreateChoiceFromCSV();
            }
            else
            {
                Debug.LogWarning("CSV 파일이 지정되지 않았습니다.");
            }
        }
        if (GUILayout.Button("Generate Dialogue"))
        {
            if (_dialogueFile != null)
            {
                CreateDialogueFromCSV();
            }
            else
            {
                Debug.LogWarning("CSV 파일이 지정되지 않았습니다.");
            }
        }
        if (GUILayout.Button("Generate NPC"))
        {
            if (_npcFile != null)
            {
                CreateNPCFromCSV();
            }
            else
            {
                Debug.LogWarning("CSV 파일이 지정되지 않았습니다.");
            }
        }
    }
    
    private void CreateQuestFromCSV()
    {
        if (_questFile == null) return;
        string[] lines = _questFile.text.Split('\n');

        string folderPath = "Assets/ScriptableObjects/Quest";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/ScriptableObjects","Quest");
        }

        for (int i = 1; i < lines.Length; i++)
        {
            // 문장의 앞,뒤 공백 제거
            string line = lines[i].Trim();

            // 공백을 제거했을 때 아무 것도 없는 경우 스킵
            if (string.IsNullOrEmpty(line)) continue;

            // 문장을 ,(쉼표)로 구분
            string[] parts = line.Split(',');
            
            QuestData quest = ScriptableObject.CreateInstance<QuestData>();
            quest.Status = QuestStatus.Locked;
            
            string questId = parts[0];
            quest.QuestID = questId;
            
            string type = parts[1];
            switch (type)
            {
                case "Talk" :
                    quest.Type = QuestType.Talk;
                    break;
                case "Delivery" :
                    quest.Type = QuestType.Delivery;
                    break;
                case "Collect" :
                    quest.Type = QuestType.Collect;
                    break;
            }
            quest.Description = parts[2];
            quest.StartNPCID = parts[3];
            quest.StartDialogueID = int.Parse(parts[4]);
            quest.EndNPCID = parts[5];
            quest.EndDialogueID = int.Parse(parts[6]);
            quest.RequiredItemID = parts[7];
            quest.RequiredItemQuantity = int.Parse(parts[8]);
            quest.RewardItemID = parts[9];
            quest.RewardItemQuantity = int.Parse(parts[10]);
            quest.NextQuestID = parts[11];
            quest.EndDescription = parts[12];// => 0으로 요청
            
            string assetPath = $"{folderPath}/Quest_{questId}.Asset";
            AssetDatabase.CreateAsset(quest, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void CreateChoiceFromCSV()
    {
        if (_questFile == null) return;
        string[] lines = _questFile.text.Split('\n');

        string folderPath = "Assets/ScriptableObjects/Choice";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/ScriptableObjects","Choice");
        }

        for (int i = 1; i < lines.Length; i++)
        {
            // 문장의 앞,뒤 공백 제거
            string line = lines[i].Trim();

            // 공백을 제거했을 때 아무 것도 없는 경우 스킵
            if (string.IsNullOrEmpty(line)) continue;

            // 문장을 ,(쉼표)로 구분
            string[] parts = line.Split(',');
            
            DialogueChoiceSO choice = ScriptableObject.CreateInstance<DialogueChoiceSO>();
            
            string choiceId = parts[0];
            choice.DialogueChoiceID = choiceId;
            
            choice.Number1 = parts[1];
            choice.NextDialogue1ID = int.Parse(parts[2]);
            choice.Number2 = parts[3];
            choice.NextDialogue2ID = int.Parse(parts[4]);
            choice.Number3 = parts[5];
            choice.NextDialogue3ID = int.Parse(parts[6]);
            
            string assetPath = $"{folderPath}/Choice_{choiceId}.Asset";
            AssetDatabase.CreateAsset(choice, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private void CreateDialogueFromCSV()
    {
    }
    
    private void CreateNPCFromCSV()
    {
    }
}
