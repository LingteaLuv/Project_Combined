using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private NPCSO _data;
    
    private List<int> _startQuest;
    private List<int> _endQuest;

    public int CurrentDialogueID { get; private set; }

    private void Awake()
    {
        _startQuest = new List<int>();
        _endQuest = new List<int>();
        Init();
    }

    private void Start()
    {
        _startQuest = QuestManager.Instance.GetStartNPC(_data.NPCID);
        _endQuest = QuestManager.Instance.GetEndNPC(_data.NPCID);
    }
    
    // 퀘스트 매니저 => 퀘스트를 관리하는 Dictionary
    public void CheckQuest(Dictionary<int,QuestData> playerQuest)
    {
        for (int i = 0; i < _startQuest.Count; i++)
        {
            if (playerQuest[_startQuest[i]].Status == QuestStatus.Available)
            {
                CurrentDialogueID = playerQuest[_startQuest[i]].StartDialogueID;
                break;
            }
            Init();
        }
        
        for (int i = 0; i < _endQuest.Count; i++)
        {
            if (playerQuest[_endQuest[i]].Status == QuestStatus.Completed)
            {
                CurrentDialogueID = playerQuest[_startQuest[i]].EndDialogueID;
                break;
                // todo : 여기서 처리하면 안되고, 실제로 플레이어가 보상을 받고 대화가 끝날 때 호출해야할듯 
                // playerQuest[playerQuest[_startQuest[i]].NextQuestID].Status = QuestStatus.Available;
            }
            Init();
        }
    }

    private void Init()
    {
        CurrentDialogueID = _data.BasicDialogueID;
    }
}
