using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] public NPCSO _data;

    private Dictionary<int, int> _dialogueFlow;
    public List<string> _startQuest;
    public List<string> _endQuest;
    
    private int _currentFlow;

    public int CurrentDialogueID { get; private set; }

    private void Awake()
    {
        _startQuest = new List<string>();
        _endQuest = new List<string>();
        Init();
    }

    private void Start()
    {
        _startQuest = QuestManager.Instance.GetStartNPC(_data.NPCID);
        _endQuest = QuestManager.Instance.GetEndNPC(_data.NPCID);
        _dialogueFlow = DialogueManager.Instance.GetDialogueFlow(_data.NPCID);
        _currentFlow = _data.BasicDialogueID;

        QuestManager.Instance.OnQuestAccepted += (data, progress) => CheckQuestAccepted(data, progress);
        QuestManager.Instance.OnQuestCompleted += (data, progress) => CheckQuestCompleted(data, progress);
    }

    private void CheckQuestAccepted(QuestData data, QuestProgress progress)
    {
        if (data.StartNPCID == _data.NPCID)
        {
            if (data.TriggerID1 == _data.Trigger1)
            {
                _currentFlow = _data.Trigger1DialogueID;
            }
            else if (data.TriggerID1 == _data.Trigger2)
            {
                _currentFlow = _data.Trigger2DialogueID;
            }
            else if (data.TriggerID1 == _data.Trigger2)
            {
                _currentFlow = _data.Trigger3DialogueID;
            }
        }
    }

    private void CheckQuestCompleted(QuestData data, QuestProgress progress)
    {
        if (data.EndNPCID == _data.NPCID)
        {
            if (data.TriggerID2 == _data.Trigger1)
            {
                _currentFlow = _data.Trigger1DialogueID;
            }
            else if (data.TriggerID2 == _data.Trigger2)
            {
                _currentFlow = _data.Trigger2DialogueID;
            }
            else if (data.TriggerID2 == _data.Trigger2)
            {
                _currentFlow = _data.Trigger3DialogueID;
            }
        }
    }
    
    // 퀘스트 매니저 => 퀘스트를 관리하는 Dictionary
    public void CheckDialogue(Dictionary<string,QuestData> playerQuest)
    {
        CheckQuest(playerQuest);
        CheckTrigger();
        Init();
    }
    private void CheckQuest(Dictionary<string,QuestData> playerQuest)
    {
        for (int i = 0; i < _startQuest.Count; i++)
        {
            if (playerQuest[_startQuest[i]].Status == QuestStatus.Available)
            {
                _currentFlow = playerQuest[_startQuest[i]].StartDialogueID;
                return;
            }
        }
        
        for (int i = 0; i < _endQuest.Count; i++)
        {
            if (playerQuest[_endQuest[i]].Status == QuestStatus.Completed)
            {
                _currentFlow = playerQuest[_endQuest[i]].EndDialogueID;
                return;
                // todo : 여기서 처리하면 안되고, 실제로 플레이어가 보상을 받고 대화가 끝날 때 호출해야할듯 
                // playerQuest[playerQuest[_startQuest[i]].NextQuestID].Status = QuestStatus.Available;
            }
        }
    }

    private void CheckTrigger()
    {
        if (!String.IsNullOrEmpty(_data.Trigger1) && DialogueManager.Instance.TriggerDic[_data.Trigger1])
        {
            _currentFlow = _data.Trigger1DialogueID;
        }
        if (!String.IsNullOrEmpty(_data.Trigger2) && DialogueManager.Instance.TriggerDic[_data.Trigger2])
        {
            _currentFlow = _data.Trigger2DialogueID;
        }
        if (!String.IsNullOrEmpty(_data.Trigger3) && DialogueManager.Instance.TriggerDic[_data.Trigger3])
        {
            _currentFlow = _data.Trigger3DialogueID;
        }
    }
    
    public void CheckLoop(int id)
    {
        if (_dialogueFlow.TryGetValue(id, out int nextDialogueID))
        {
            _currentFlow = nextDialogueID;
        }
    }

    private void Init()
    {
        CurrentDialogueID = _currentFlow;
    }
}
