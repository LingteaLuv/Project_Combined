using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public NPCSO Data { get; }
    
    private List<int> _startQuest;
    private List<int> _endQuest;
    
    public void GetStartQuestID()
    {
        //퀘스트 매니저한테 퀘스트를 받아서 설정
    }

    private void Awake()
    {
        _startQuest = new List<int>();
        _endQuest = new List<int>();
    }
    

    // 퀘스트 매니저 => 퀘스트를 관리하는 Dictionary
    private void CheckQuest(Dictionary<int,QuestData> playerQuest)
    {
        for (int i = 0; i < _startQuest.Count; i++)
        {
            if (playerQuest[_startQuest[i]].Status == QuestStatus.Available)
            {
                DialogueManager.Instance.GetDialogue(playerQuest[_startQuest[i]].StartDialogueID);
            }
        }
        
        for (int i = 0; i < _endQuest.Count; i++)
        {
            if (playerQuest[_endQuest[i]].Status == QuestStatus.Completed)
            {
                DialogueManager.Instance.GetDialogue(playerQuest[_startQuest[i]].EndDialogueID);
                playerQuest[playerQuest[_startQuest[i]].NextQuestID].Status = QuestStatus.Available;
            }
        }
    }
    
// 퀘스트 ID => Available
// 얘의 대사 출력이 바뀌는
// if(id 검사해서 Available이면)
// DialogueManager.Instance.GetDialogue(StartDialogueID)

// if(id 검사해서 Completed이면)
// DialogueManager.Instance.GetDialogue(EndDialogueID)
}
