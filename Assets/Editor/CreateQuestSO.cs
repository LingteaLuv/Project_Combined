using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

public class CreateQuestSO : EditorWindow
{
    private TextAsset _questFile;
    private TextAsset _choiceFile;
    private TextAsset _dialogueFile;
    private TextAsset _npcFile;

    [MenuItem("Tools/CSV to Quest Dialogue NPC")]
    public static void ShowWindow()
    {
        GetWindow<CreateQuestSO>("CSV to Quest Dialogue NPC");
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
            quest.Status = QuestStatus.Available;
            
            string questId = parts[0];
            quest.QuestID = questId;
            
            string type = parts[1];
            switch (type)
            {
                case "GiveItem" :
                    quest.Type = QuestType.Collect;
                    break;
                case "Delivery" :
                    quest.Type = QuestType.Delivery;
                    break;
                case "Recall" :
                    quest.Type = QuestType.Collect;
                    break;
            }
            quest.Description = parts[2];
            quest.StartNPCID = parts[3];
            quest.StartDialogueID = int.Parse(parts[4]);
            quest.EndNPCID = parts[5];
            quest.EndDialogueID = int.Parse(parts[6]);
            quest.RequiredItemID = parts[7];
            if(int.TryParse(parts[8], out int requiredQuantity))
            {
                quest.RequiredItemQuantity = requiredQuantity;
            }
            quest.RewardItemID = parts[9];
            if(int.TryParse(parts[10], out int rewardQuantity))
            {
                quest.RewardItemQuantity = rewardQuantity;
            }
            quest.NextQuestID = parts[11];
            quest.EndDescription = parts[12];
            quest.DeliveryNpcID = parts[13];
            quest.TriggerID1 = parts[14];
            quest.TriggerID2 = parts[15];
            
            string assetPath = $"{folderPath}/Quest_{questId}.Asset";
            AssetDatabase.CreateAsset(quest, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void CreateChoiceFromCSV()
    {
        if (_choiceFile == null) return;
        string[] lines = _choiceFile.text.Split('\n');

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
            
            choice.Number1 = parts[1].Replace(";", ",");
            if(int.TryParse(parts[2], out int next1))
            {
                choice.NextDialogue1ID = next1;
            }
            else
            {
                choice.NextDialogue1ID = 0;
            }
            choice.Number2 = parts[3].Replace(";", ",");
            if(int.TryParse(parts[4], out int next2))
            {
                choice.NextDialogue2ID = next2;
            }
            else
            {
                choice.NextDialogue2ID = 0;
            }
            choice.Number3 = parts[5].Replace(";", ",");
            if(int.TryParse(parts[6], out int next3))
            {
                choice.NextDialogue3ID = next3;
            }
            else
            {
                choice.NextDialogue3ID = 0;
            }
            
            string assetPath = $"{folderPath}/Choice_{choiceId}.Asset";
            AssetDatabase.CreateAsset(choice, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private void CreateDialogueFromCSV()
    {
        if (_dialogueFile == null) return;
        string[] lines = _dialogueFile.text.Split('\n');

        string folderPath = "Assets/ScriptableObjects/Dialogue";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/ScriptableObjects","Dialogue");
        }

        for (int i = 1; i < lines.Length; i++)
        {
            // 문장의 앞,뒤 공백 제거
            string line = lines[i].Trim();

            // 공백을 제거했을 때 아무 것도 없는 경우 스킵
            if (string.IsNullOrEmpty(line)) continue;

            // 문장을 ,(쉼표)로 구분
            string[] parts = line.Split(',');
            
            DialogueSO dialogue = ScriptableObject.CreateInstance<DialogueSO>();
            
            string dialogueId = parts[0];
            dialogue.DialogueID = int.Parse(dialogueId);
            dialogue.NPCID = parts[1];
            dialogue.DialogueText = parts[2].Replace(";", ",");
            dialogue.DialogueChoiceID = parts[3];
            if(int.TryParse(parts[4], out int loop))
            {
                dialogue.LoopDialogueID = loop;
            }
            else
            {
                dialogue.LoopDialogueID = 0;
            }
            if (parts[5] == "TRUE")
            {
                dialogue.EndCheck = true;
            }
            else
            {
                dialogue.EndCheck = false;
            }
            
            dialogue.TriggerID = parts[6];
            
            string assetPath = $"{folderPath}/Dialogue_{dialogueId}.Asset";
            AssetDatabase.CreateAsset(dialogue, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private void CreateNPCFromCSV()
    {
        if (_npcFile == null) return;
        string[] lines = _npcFile.text.Split('\n');

        string folderPath = "Assets/ScriptableObjects/NPC";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/ScriptableObjects","NPC");
        }

        for (int i = 1; i < lines.Length; i++)
        {
            // 문장의 앞,뒤 공백 제거
            string line = lines[i].Trim();

            // 공백을 제거했을 때 아무 것도 없는 경우 스킵
            if (string.IsNullOrEmpty(line)) continue;

            // 문장을 ,(쉼표)로 구분
            string[] parts = line.Split(',');
            
            NPCSO npc = ScriptableObject.CreateInstance<NPCSO>();
            
            string npcID = parts[0];
            npc.NPCID = npcID;
            npc.Name = parts[1];
            if (int.TryParse(parts[3], out int basicID))
            {
                npc.BasicDialogueID = basicID;
            }
            npc.Trigger1 = parts[4];
            if (int.TryParse(parts[5], out int trigger1Id))
            {
                npc.Trigger1DialogueID = trigger1Id;
            }
            else
            {
                npc.Trigger1DialogueID = 0;
            }
            npc.Trigger2 = parts[6];
            if (int.TryParse(parts[7], out int trigger2Id))
            {
                npc.Trigger2DialogueID = trigger2Id;
            }
            else
            {
                npc.Trigger2DialogueID = 0;
            }
            npc.Trigger3 = parts[8];
            if (int.TryParse(parts[9], out int trigger3Id))
            {
                npc.Trigger3DialogueID = trigger3Id;
            }
            else
            {
                npc.Trigger3DialogueID = 0;
            }
            if (int.TryParse(parts[10], out int startId))
            {
                npc.StartQuestID = startId;
            }
            else
            {
                npc.StartQuestID = 0;
            }
            
            string assetPath = $"{folderPath}/Choice_{npcID}.Asset";
            AssetDatabase.CreateAsset(npc, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
