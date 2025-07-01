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
    /// 전체 퀘스트의 메타(설계) 데이터 리스트입니다.
    /// </summary>
    public List<QuestData> AllQuests { get; private set; } = new List<QuestData>();

    /// <summary>
    /// 전체 퀘스트의 플레이어별 진행 상태 리스트입니다.
    /// </summary>
    private Dictionary<string, QuestData> _questDictionary = new Dictionary<string, QuestData>();

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
    /// 전체 퀘스트의 메타(설계) 데이터 리스트를 등록/초기화합니다.
    /// </summary>
    /// <param name="questList">등록할 퀘스트 메타 데이터 리스트</param>
    public void LoadQuestMeta(List<QuestData> questList)
    {
        AllQuests = questList ?? new List<QuestData>();
        _questDictionary = AllQuests.ToDictionary(q => q.QuestID);
        //UpdateQuestStates();
    }

    /// <summary>
    /// 전체 퀘스트의 플레이어별 진행 데이터 리스트를 등록/초기화합니다.
    /// </summary>
    /// <param name="questId">수락할 퀘스트 ID</param>
    /// <returns>성공 시 true</returns>
    public bool AcceptQuest(string questId)
    {
        var meta = GetQuestMeta(questId);
        var progress = GetProgress(questId);
        if (meta == null || progress == null || progress.Status != QuestStatus.Available)
            return false;
        progress.Status = QuestStatus.Active;
        OnQuestAccepted?.Invoke(meta, progress);
        SaveProgress();
        return true;
    }

    /// <summary>
    /// 퀘스트를 완료(Completed) 상태로 변경합니다.
    /// </summary>
    /// <param name="questId">완료할 퀘스트 ID</param>
    /// <returns>성공 시 true</returns>
    public bool CompleteQuest(string questId)
    {
        var meta = GetQuestMeta(questId);
        var progress = GetProgress(questId);
        if (meta == null || progress == null || progress.Status != QuestStatus.Active)
            return false;

        quest.Status = QuestStatus.Completed;
        OnQuestCompleted?.Invoke(quest);
        // TODO: 보상 지급, 후속 퀘스트 해금, 세이브 등
        //UpdateQuestStates(); // 완료 후 해금 갱신
        return true;
    }

    /// <summary>
    /// 퀘스트 보상을 수령 처리합니다.
    /// </summary>
    /// <param name="questId">실패 처리할 퀘스트 ID</param>
    /// <returns>성공 시 true</returns>
    public bool FailQuest(string questId)
    {
        var meta = GetQuestMeta(questId);
        var progress = GetProgress(questId);
        if (meta == null || progress == null || progress.Status != QuestStatus.Completed || progress.IsRewardClaimed)
            return false;
        progress.IsRewardClaimed = true;
        OnQuestRewardClaimed?.Invoke(meta, progress);
        SaveProgress();
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
            // TODO: 해금 알림, UI 표시 등
        }
    }

    /// <summary>
    /// 해당 퀘스트가 해금(Available) 조건을 만족하는지 판별합니다.
    /// </summary>
    public bool IsQuestCompleted(string questId)
    {
        var progress = GetProgress(questId);
        return progress != null && progress.Status == QuestStatus.Completed;
    }

    /// <summary>
    /// 퀘스트 목표 진행(GoalCount) 증가. 처치/수집 등에서 호출.
    /// </summary>
    private QuestData GetQuestById(string questId)
    {
        var progress = GetProgress(questId);
        var meta = GetQuestMeta(questId);
        if (progress == null || meta == null || progress.Status != QuestStatus.Active)
            return;
        progress.GoalCount += amount;
        if (meta.Type == QuestType.Kill && progress.GoalCount >= meta.TargetMonsterCount)
            CompleteQuest(questId);
        SaveProgress();
    }

    /// <summary>
    /// 퀘스트 진행 정보를 Json 파일로 저장합니다.
    /// </summary>
    //private bool CanUnlockQuest(QuestData quest)
}
