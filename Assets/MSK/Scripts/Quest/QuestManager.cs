using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 게임 내 모든 퀘스트를 통합 관리하는 싱글톤 매니저 클래스입니다.
/// </summary>
public class QuestManager : Singleton<QuestManager>
{
    /// <summary>
    /// 전체 퀘스트 데이터(초기화 시 로드)
    /// </summary>
    public List<QuestData> AllQuests { get; private set; } = new List<QuestData>();

    /// <summary>
    /// 퀘스트 ID로 빠르게 접근할 수 있는 캐시 딕셔너리
    /// </summary>
    private Dictionary<string, QuestData> _questDictionary = new Dictionary<string, QuestData>();

    /// <summary>
    /// 플레이어의 현재 챕터(비트 플래그)
    /// </summary>
    public Chapter CurrentChapter { get; set; }

    #region 이벤트 (외부 시스템 연동/알림용)
    /// <summary>퀘스트 수락 시 발행</summary>
    public event Action<QuestData> OnQuestAccepted;
    /// <summary>퀘스트 완료 시 발행</summary>
    public event Action<QuestData> OnQuestCompleted;
    /// <summary>퀘스트 실패 시 발행</summary>
    public event Action<QuestData> OnQuestFailed;
    #endregion

    #region 상태별 퀘스트 리스트
    /// <summary>상태별 퀘스트 리스트 반환</summary>
    public List<QuestData> GetQuestsByStatus(QuestStatus status)
        => AllQuests.Where(q => q.Status == status).ToList();

    /// <summary>진행 가능(Available) 퀘스트</summary>
    public List<QuestData> AvailableQuests => GetQuestsByStatus(QuestStatus.Available);
    /// <summary>진행 중(Active) 퀘스트</summary>
    public List<QuestData> ActiveQuests => GetQuestsByStatus(QuestStatus.Active);
    /// <summary>완료(Completed) 퀘스트</summary>
    public List<QuestData> CompletedQuests => GetQuestsByStatus(QuestStatus.Completed);
    #endregion

    /// <summary>
    /// 퀘스트 데이터를 로드하고, 내부 딕셔너리와 상태를 초기화합니다.
    /// </summary>
    /// <param name="questList">로드할 전체 퀘스트 리스트</param>
    public void LoadQuests(List<QuestData> questList)
    {
        AllQuests = questList ?? new List<QuestData>();
        //_questDictionary = AllQuests.ToDictionary(q => q.QuestID);
        //UpdateQuestStates();
    }

    /// <summary>
    /// 퀘스트를 수락 처리합니다.
    /// </summary>
    /// <param name="questId">수락할 퀘스트 ID</param>
    /// <returns>성공 시 true</returns>
    public bool AcceptQuest(string questId)
    {
        var quest = GetQuestById(questId);
        if (quest == null || quest.Status != QuestStatus.Available)
            return false;

        quest.Status = QuestStatus.Active;
        OnQuestAccepted?.Invoke(quest);
        // TODO: 세이브/알림/추적 시작 등 추가 처리 추가
        return true;
    }

    /// <summary>
    /// 퀘스트 완료 처리.
    /// </summary>
    /// <param name="questId">완료할 퀘스트 ID</param>
    /// <returns>성공 시 true</returns>
    public bool CompleteQuest(string questId)
    {
        var quest = GetQuestById(questId);
        if (quest == null || quest.Status != QuestStatus.Active)
            return false;

        quest.Status = QuestStatus.Completed;
        OnQuestCompleted?.Invoke(quest);
        // TODO: 보상 지급, 후속 퀘스트 해금, 세이브 등
        //UpdateQuestStates(); // 완료 후 해금 갱신
        return true;
    }

    /// <summary>
    /// 퀘스트 실패 처리.
    /// </summary>
    /// <param name="questId">실패 처리할 퀘스트 ID</param>
    /// <returns>성공 시 true</returns>
    public bool FailQuest(string questId)
    {
        var quest = GetQuestById(questId);
        if (quest == null || quest.Status != QuestStatus.Active)
            return false;

        quest.Status = QuestStatus.Failed;
        OnQuestFailed?.Invoke(quest);
        return true;
    }

    /// <summary>
    /// 챕터 진입/선행퀘스트 클리어 등 변화에 따라
    /// 잠긴 퀘스트를 해금(Available) 처리합니다.
    /// </summary>
    public void UpdateQuestStates(QuestData quest)
    {
        if (quest.Status == QuestStatus.Locked)
        {
            quest.Status = QuestStatus.Available;
            // TODO: 해금 알림, UI 표시 등
        }
    }

    /// <summary>
    /// 퀘스트 완료 여부를 반환합니다.
    /// </summary>
    public bool IsQuestCompleted(string questId)
    {
        var quest = GetQuestById(questId);
        return quest != null && quest.Status == QuestStatus.Completed;
    }

    /// <summary>
    /// QuestID로 퀘스트를 빠르게 조회합니다.
    /// </summary>
    private QuestData GetQuestById(string questId)
    {
        if (_questDictionary == null) return null;
        _questDictionary.TryGetValue(questId, out var quest);
        return quest;
    }

    /// <summary>
    /// 챕터, 선행퀘스트 등 해금 조건을 만족하는지 검사합니다.
    /// </summary>
    //private bool CanUnlockQuest(QuestData quest)
}
