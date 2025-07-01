using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC")]
public class NPCSO : ScriptableObject
{
    public string NPCID;
    // 등장조건 
    public string NPCTriggerID;
    public string BasicDialogueID;
    
    public string Trigger1;
    public string Trigger1Dialogue;
    public string Trigger2;
    public string Trigger2Dialogue;
    public string Trigger3;
    public string Trigger3Dialogue;

    public string StartQusetID;
    public string TriggerQuestID;
    public string QuestTriggerID;

    public string StartDialogueID;
    
    public string EndDialogueID;

    private List<string> startQuest;
    private List<string> EndQuest;
    
    public void GetStartQuestID()
    {
        //퀘스트 매니저한테 퀘스트를 받아서 설정
    }

    private void Awake()
    {
        startQuest = new List<string>();
        EndQuest = new List<string>();
    }

    // 퀘스트 매니저 => 퀘스트를 관리하는 Dictionary
    private void Print(Dictionary<string,QuestData> playerQuest)
    {
        for (int i = 0; i < startQuest.Count; i++)
        {
            if (playerQuest[startQuest[i]].Status == QuestStatus.Available)
            {
                DialogueManager.Instance.GetDialogue(playerQuest[startQuest[i]].StartDialogueID);
            }
        }
        
        for (int i = 0; i < EndQuest.Count; i++)
        {
            if (playerQuest[EndQuest[i]].Status == QuestStatus.Completed)
            {
                DialogueManager.Instance.GetDialogue(playerQuest[startQuest[i]].EndDialogueID);
                playerQuest[playerQuest[startQuest[i]].NextQuestID].Status = QuestStatus.Available;
            }
        }
    }
}

// 퀘스트 ID => Available
// 얘의 대사 출력이 바뀌는
// if(id 검사해서 Available이면)
// DialogueManager.Instance.GetDialogue(StartDialogueID)

// if(id 검사해서 Completed이면)
// DialogueManager.Instance.GetDialogue(EndDialogueID)
