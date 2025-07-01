using System;
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
    public List<QuestData> AllQuests { get; private set; } = new List<QuestData>();

    /// <summary>
    /// 전체 퀘스트의 플레이어별 진행 상태 딕셔너리입니다.
    /// </summary>
    private Dictionary<int, QuestData> _questDictionary = new Dictionary<int, QuestData>();

    /// <summary>
    /// 플레이어의 현재 챕터(비트 플래그)
    /// </summary>
    public Chapter CurrentChapter { get; set; } = Chapter.Chapter1;

    #region 이벤트
    /// <summary>퀘스트 수락 시 알림 이벤트</summary>
    public event Action<QuestData, QuestProgress> OnQuestAccepted;
    /// <summary>퀘스트 완료 시 알림 이벤트</summary>
    public event Action<QuestData, QuestProgress> OnQuestCompleted;
    /// <summary>퀘스트 보상 수령 시 알림 이벤트</summary>
    public event Action<QuestData, QuestProgress> OnQuestRewardClaimed;
    #endregion


    /// <summary>
    /// Npc에게 퀘스트를 Npc 리스트로 전달
    /// </summary>
    public List<QuestData> GetStartNPC(int npcId)
    {
        return _questDictionary.Values.Where(q => q.StartNPCID == npcId).ToList();
    }
    public List<QuestData> GetEndNPC(int npcId)
    {
        return _questDictionary.Values.Where(q => q.EndNPCID == npcId).ToList();
    }


    /// <summary>
    /// 연계 퀘스트가 있다면, 상태를 Locked로 초기화합니다.
    /// (NextQuestID가 존재하는 모든 퀘스트에 대해 후속 퀘스트를 잠금 처리)
    /// </summary>
    public void InitQuestLock()
    {
        foreach (var quest in _questDictionary.Values)
        {
            // NextQuestID가 존재하는 경우
            if (quest.NextQuestID != 0)
            {
                // 딕셔너리에서 다음 퀘스트를 탐색
                if (_questDictionary.TryGetValue(quest.NextQuestID, out var nextQuest))
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
        AllQuests = questList ?? new List<QuestData>();
        //_questDictionary = AllQuests.ToDictionary(q => q.QuestID);
        //UpdateQuestStates();
    }

    /// <summary>
    /// 전체 퀘스트의 플레이어별 진행 데이터 리스트를 등록/초기화합니다.
    /// </summary>
    /// <param name="questId">수락할 퀘스트 ID</param>
    /// <returns>성공 시 true</returns>
    public bool AcceptQuest(int questId)
    {
        if (!_questDictionary.TryGetValue(questId, out var meta))
            return false;
        if (meta.Status != QuestStatus.Available)
            return false;
        meta.Status = QuestStatus.Active;
        OnQuestAccepted?.Invoke(meta, null);
        return true;
    }

    /// <summary>
    /// 퀘스트를 완료(Completed) 상태로 변경합니다.
    /// </summary>
    /// <param name="questId">완료할 퀘스트 ID</param>
    /// <returns>성공 시 true</returns>
    public bool CompleteQuest(int questId)
    {
        if (!_questDictionary.TryGetValue(questId, out var meta))
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
    public bool CloseQuest(int questId)
    {
        if (!_questDictionary.TryGetValue(questId, out var meta))
            return false;
        if (meta.Status != QuestStatus.Completed)
            return false;

        meta.Status = QuestStatus.Closed;

        // NextQuestID가 null/빈문자열이 아니면 다음 퀘스트 해금 시도
        if (meta.NextQuestID != 0 &&
            _questDictionary.TryGetValue(meta.NextQuestID, out var nextQuest))
        {
            UpdateQuestStates(nextQuest);
        }

        return true;
    }

    /// <summary>
    /// 퀘스트 보상을 수령 처리합니다.
    /// </summary>
    /// <param name="questId">실패 처리할 퀘스트 ID</param>
    /// <returns>성공 시 true</returns>
    public bool FailQuest(int questId)
    {
        if (!_questDictionary.TryGetValue(questId, out var meta))
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
    public bool IsQuestCompleted(int questId)
    {
        return _questDictionary.TryGetValue(questId, out var meta) && meta.Status == QuestStatus.Completed;
    }

    /// <summary>
    /// 퀘스트 목표 진행(GoalCount) 증가. 처치/수집 등에서 호출.
    /// </summary>
    public void GetQuestById(int questId, int amount)
    {
        if (!_questDictionary.TryGetValue(questId, out var meta))
            return;
        if (meta.Status != QuestStatus.Active)
            return;
        CompleteQuest(questId);
    }
}
