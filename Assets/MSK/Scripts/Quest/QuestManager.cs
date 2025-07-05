using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 게임 내 모든 퀘스트의 메타데이터와 진행 정보를 통합 관리하는 싱글톤 매니저 클래스입니다.
/// - 퀘스트 등록/조회/상태 변화/보상/저장/불러오기 등 처리
/// - Singleton 패턴 적용(Instance)
/// </summary>
public class QuestManager : Singleton<QuestManager>
{
    /// <summary>
    /// 전체 퀘스트의 데이터 리스트입니다.
    /// </summary>
    [SerializeField] private List<QuestData> allQuests;
    public List<QuestData> AllQuests => allQuests;

    /// <summary>
    /// 전체 퀘스트의 플레이어별 진행 상태 딕셔너리입니다.
    /// </summary>
    [SerializeField] public Dictionary<string, QuestData> QuestDictionary { get; private set; } = new Dictionary<string, QuestData>(){};

    /// <summary>
    /// 전체 트리거와 매칭되는 퀘스트ID를 저장하는 딕셔너리입니다.
    /// </summary>
    public Dictionary<string, string> TriggerDictionary { get; private set; }
    /// <summary>
    /// 플레이어의 현재 챕터(비트 플래그)
    /// </summary>
    public Chapter CurrentChapter { get; set; } = Chapter.Chapter1;

    #region 이벤트
    /// <summary>퀘스트 수락 시 알림 이벤트</summary>
    public event Action<QuestData, QuestProgress> OnQuestAccepted;
    /// <summary>퀘스트 완료 시 알림 이벤트</summary>
    public event Action<QuestData, QuestProgress> OnQuestCompleted;
    /// <summary>퀘스트 종료 시 알림 이벤트</summary>
    public event Action<QuestData, QuestProgress> OnQuestClosed;
    /// <summary>퀘스트 보상 수령 시 알림 이벤트</summary>
    public event Action<QuestData, QuestProgress> OnQuestRewardClaimed;
    #endregion

    public List<QuestData> AcceptedItemQuestList { get; set; } = new List<QuestData>(); //추가 작성된 부분


    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        SetQuestDictionary();
        TriggerDictionary = new Dictionary<string, string>();
        for (int i = 0; i < AllQuests.Count; i++)
        {
            if (String.IsNullOrEmpty(AllQuests[i].TriggerID1)||String.IsNullOrEmpty(AllQuests[i].TriggerID2)) continue;
            TriggerDictionary.Add(AllQuests[i].TriggerID1, AllQuests[i].QuestID);
            TriggerDictionary.Add(AllQuests[i].TriggerID2, AllQuests[i].QuestID);
        }
    }
    
    /// <summary>
    /// Npc에게 조건에 부합하는 퀘스트를 Npc 리스트로 전달 => key를 전달하도록 수정
    /// </summary>
    public List<string> GetStartNPC(string npcId)
    {
        return QuestDictionary.Keys.Where(q => QuestDictionary[q].StartNPCID == npcId).ToList();
    }
    public List<string> GetEndNPC(string npcId)
    {
        return QuestDictionary.Keys.Where(q => QuestDictionary[q].EndNPCID == npcId).ToList();
    }
    public void SetQuestType(string triggerId)
    {
        // 트리거명으로 QuestID를 찾기
        if (!TriggerDictionary.TryGetValue(triggerId, out var questId))
        {
            Debug.LogWarning($"[QuestType] TriggerDictionary에 triggerId({triggerId}) 없음");
            return;
        }

        // QuestID로 QuestDictionary에서 QuestData를 찾기
        if (!QuestDictionary.TryGetValue(questId, out var meta))
        {
            Debug.LogWarning($"[QuestType] QuestDictionary에 questId({questId}) 없음");
            return;
        }

        Debug.Log($"[QuestType] questId: {questId}, 현재 상태: {meta.Status}");

        switch (meta.Status)
        {
            case QuestStatus.Locked:
                Debug.Log("[QuestType] Locked");
                break;
            case QuestStatus.Available:
                Debug.Log("[QuestType] Available");
                AcceptQuest(meta.QuestID);
                break;
            case QuestStatus.Active:
                Debug.Log("[QuestType] Active");
                CompleteQuest(meta.QuestID);
                break;
            case QuestStatus.Completed:
                Debug.Log("[QuestType] Completed");
                CloseQuest(meta.QuestID);
                break;
            case QuestStatus.Closed:
                Debug.Log("[QuestType] Closed");
                break;
            default:
                Debug.Log("[QuestType] 기타");
                break;
        }
    }


    /// <summary>
    /// 연계 퀘스트가 있다면, 상태를 Locked로 초기화합니다.
    /// (NextQuestID가 존재하는 모든 퀘스트에 대해 후속 퀘스트를 잠금 처리)
    /// </summary>
    public void InitQuestLock()
    {
        foreach (var quest in QuestDictionary.Values)
        {
            // NextQuestID가 존재하는 경우
            if (!String.IsNullOrEmpty(quest.NextQuestID))
            {
                // 딕셔너리에서 다음 퀘스트를 탐색
                if (QuestDictionary.TryGetValue(quest.NextQuestID, out var nextQuest))
                {
                    // 상태를 Locked로 설정
                    nextQuest.Status = QuestStatus.Locked;
                }
            }
        }
    }
    
    /// <summary>
    /// 전체 퀘스트의 데이터 리스트를 등록/초기화합니다.
    /// </summary>
    /// <param name="questList">등록할 퀘스트 데이터 리스트</param>
    public void LoadQuest(List<QuestData> questList)
    {
        allQuests = questList ?? new List<QuestData>();
        //_questDictionary = AllQuests.ToDictionary(q => q.QuestID);
        //UpdateQuestStates();
    }

    /// <summary>
    /// 전체 퀘스트의 플레이어별 진행 데이터 리스트를 등록/초기화합니다.
    /// </summary>
    /// <param name="questId">수락할 퀘스트 ID</param>
    /// <returns>성공 시 true</returns>
    public bool AcceptQuest(string questId)
    {
        if (!QuestDictionary.TryGetValue(questId, out var meta))
            return false;
        if (meta.Status != QuestStatus.Available)
            return false;
        if (meta.Type == QuestType.Delivery && meta.RequiredItemQuantity > InventoryManager.Instance.GetNullSpaceCount())
        {
            TextManager.Instance.PopupTextForSecond("2001", 3);
            return false;
        }
        meta.Status = QuestStatus.Active;

        CheckItemQuest(meta); //추가 작성된 부분

        OnQuestAccepted?.Invoke(meta, null);
        return true;
    }

    private void CheckItemQuest(QuestData meta) //추가 작성된 부분
    {
        if(meta.Type == QuestType.Delivery)
        {
            AcceptedItemQuestList.Add(meta);
            int.TryParse(meta.RequiredItemID, out int req);
            if (meta.RequiredItemQuantity == 0) // 배달 요구하는 아이템이 0개면, 아이템을 어딘가에 두고 와야 하는 퀘스트
            {
                InventoryManager.Instance.AddItemByID(req, meta.RequiredItemQuantity + 1);
            }
            else
            {
                InventoryManager.Instance.AddItemByID(req, meta.RequiredItemQuantity);
            }
        }
        else if (meta.Type == QuestType.Collect)
        {
            AcceptedItemQuestList.Add(meta);
        }
    }

    /// <summary>
    /// 퀘스트를 완료(Completed) 상태로 변경합니다.
    /// </summary>
    /// <param name="questId">완료할 퀘스트 ID</param>
    /// <returns>성공 시 true</returns>
    public bool CompleteQuest(string questId)
    {
        if (!QuestDictionary.TryGetValue(questId, out var meta))
            return false;
        if (meta.Status != QuestStatus.Active)
            return false;

        meta.Status = QuestStatus.Completed;
        OnQuestCompleted?.Invoke(meta, null);

        // TODO: 보상 지급
        
        return true;
    }
    /// <summary>
    /// 퀘스트를 종료(Closed) 상태로 변경하고, 연계 퀘스트가 있다면 해금합니다.
    /// </summary>
    /// <param name="questId">종료할 퀘스트 ID</param>
    /// <returns>성공 시 true</returns>
    public bool CloseQuest(string questId)
    {
        if (!QuestDictionary.TryGetValue(questId, out var meta))
            return false;
        if (meta.Status != QuestStatus.Completed)
            return false;
        if (meta.RewardItemQuantity > InventoryManager.Instance.GetNullSpaceCount())
        {
            TextManager.Instance.PopupTextForSecond("2002", 3);
            return false;
        }

        meta.Status = QuestStatus.Closed;
        int.TryParse(meta.RequiredItemID, out int req);
        InventoryManager.Instance.RemoveItemByID(req, meta.RequiredItemQuantity);
        int.TryParse(meta.RewardItemID, out int rew); // 추가 작성된 부분
        InventoryManager.Instance.AddItemByID(rew, meta.RewardItemQuantity); // 추가 작성된 부분

        // NextQuestID가 null/빈문자열이 아니면 다음 퀘스트 해금 시도
        if (!String.IsNullOrEmpty(meta.NextQuestID) &&
                                  QuestDictionary.TryGetValue(meta.NextQuestID, out var nextQuest))
        {
            UpdateQuestStates(nextQuest);
        }
        OnQuestClosed?.Invoke(meta, null);
        return true;
    }

    /// <summary>
    /// 퀘스트 보상을 수령 처리합니다.
    /// </summary>
    /// <param name="questId">실패 처리할 퀘스트 ID</param>
    /// <returns>성공 시 true</returns>
    public bool FailQuest(string questId)
    {
        if (!QuestDictionary.TryGetValue(questId, out var meta))
            return false;
        if (meta.Status != QuestStatus.Completed)
            return false;
        OnQuestRewardClaimed?.Invoke(meta, null);
        return true;
    }

    /// <summary>
    /// 챕터/선행퀘스트 해금 조건을 만족하는 퀘스트를 Available로 갱신합니다.
    /// </summary>
    public void UpdateQuestStates(QuestData quest)
    {
        if (quest.Status == QuestStatus.Locked)
        {
            quest.Status = QuestStatus.Available;
        }
    }

    /// <summary>
    /// 해당 퀘스트가 해금(Available) 조건을 만족하는지 판별합니다.
    /// </summary>
    public bool IsQuestCompleted(string questId)
    {
        return QuestDictionary.TryGetValue(questId, out var meta) && meta.Status == QuestStatus.Completed;
    }

    /// <summary>
    /// 퀘스트 목표 진행(GoalCount) 증가. 처치/수집 등에서 호출.
    /// </summary>
    public void GetQuestById(string questId, int amount)
    {
        if (!QuestDictionary.TryGetValue(questId, out var meta))
            return;
        if (meta.Status != QuestStatus.Active)
            return;
        CompleteQuest(questId);
    }

    /// <summary>
    /// NPC의 ID로 해당 NPC가 표시해야 할 퀘스트 상태를 반환합니다.
    /// 완료 NPC(EndNPCID)가 우선이며, 그 후 수주 NPC(StartNPCID)를 검사합니다.s
    /// </summary>
    public QuestStatus? GetNpcQuestStatus(string npcId)
    {
        var quests = QuestDictionary.Values;

        //  EndNPCID
        var endQuest = quests.FirstOrDefault(q => q.EndNPCID == npcId);
        if (endQuest != null)
        {
            switch (endQuest.Status)
            {
                case QuestStatus.Completed:  // 완료 가능
                    return QuestStatus.Completed;
                case QuestStatus.Active:     // 진행 중
                    return QuestStatus.Active;
                default:
                    break;
            }
        }

        // StartNPCID
        var startQuest = quests.FirstOrDefault(q => q.StartNPCID == npcId);
        if (startQuest != null)
        {
            switch (startQuest.Status)
            {
                case QuestStatus.Available:  // 수주 가능
                    return QuestStatus.Available;
                case QuestStatus.Active:     // 진행 중
                    return QuestStatus.Active;
                default:
                    return null;
            }
        }
        return null;
    }
    private void SetQuestDictionary()
    {
        QuestDictionary = new Dictionary<string, QuestData>();
        foreach (var quest in AllQuests)
        {
            QuestDictionary[quest.QuestID] = quest;
        }

        Debug.Log($"[SetQuestDictionary] QuestDictionary.Count = {QuestDictionary.Count}");
    }
}
