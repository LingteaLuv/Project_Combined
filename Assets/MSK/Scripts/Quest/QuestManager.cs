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
    public List<QuestProgress> ProgressList { get; private set; } = new List<QuestProgress>();

    // 내부 캐싱용 딕셔너리 (ID→퀘스트)
    private Dictionary<int, QuestData> questMetaDict = new Dictionary<int, QuestData>();
    private Dictionary<int, QuestProgress> questProgressDict = new Dictionary<int, QuestProgress>();

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
        questMetaDict = AllQuests.ToDictionary(q => q.QuestID);
    }

    /// <summary>
    /// 전체 퀘스트의 플레이어별 진행 데이터 리스트를 등록/초기화합니다.
    /// </summary>
    /// <param name="progressList">등록할 진행 데이터 리스트</param>
    public void LoadQuestProgress(List<QuestProgress> progressList)
    {
        ProgressList = progressList ?? new List<QuestProgress>();
        questProgressDict = ProgressList.ToDictionary(qp => qp.QuestID);
    }

    /// <summary>
    /// 퀘스트 메타(설계) 정보를 퀘스트ID로 반환합니다.
    /// </summary>
    /// <param name="questId">퀘스트ID</param>
    /// <returns>QuestData 또는 null</returns>
    public QuestData GetQuestMeta(int questId) =>
        questMetaDict.TryGetValue(questId, out var meta) ? meta : null;

    /// <summary>
    /// 퀘스트 진행(플레이어 상태) 정보를 퀘스트ID로 반환합니다.
    /// </summary>
    /// <param name="questId">퀘스트ID</param>
    /// <returns>QuestProgress 또는 null</returns>
    public QuestProgress GetProgress(int questId) =>
        questProgressDict.TryGetValue(questId, out var progress) ? progress : null;

    /// <summary>
    /// 상태별(예: Available/Completed 등) 퀘스트 메타 리스트를 반환합니다.
    /// </summary>
    /// <param name="status">필터할 퀘스트 상태</param>
    /// <returns>상태별 QuestData 리스트</returns>
    public List<QuestData> GetQuestsByStatus(QuestStatus status)
    {
        return ProgressList
            .Where(qp => qp.Status == status)
            .Select(qp => GetQuestMeta(qp.QuestID)).Where(meta => meta != null).ToList();
    }

    /// <summary>
    /// 퀘스트를 수락(진행 상태로 변경)합니다.
    /// </summary>
    /// <param name="questId">수락할 퀘스트ID</param>
    /// <returns>성공 시 true, 실패 시 false</returns>
    public bool AcceptQuest(int questId)
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
    /// <param name="questId">완료할 퀘스트ID</param>
    /// <returns>성공 시 true, 실패 시 false</returns>
    public bool CompleteQuest(int questId)
    {
        var meta = GetQuestMeta(questId);
        var progress = GetProgress(questId);
        if (meta == null || progress == null || progress.Status != QuestStatus.Active)
            return false;
        progress.Status = QuestStatus.Completed;
        OnQuestCompleted?.Invoke(meta, progress);
        UpdateQuestUnlockStates();
        SaveProgress();
        return true;
    }

    /// <summary>
    /// 퀘스트 보상을 수령 처리합니다.
    /// </summary>
    /// <param name="questId">보상받을 퀘스트ID</param>
    /// <returns>성공 시 true, 실패 시 false</returns>
    public bool ClaimReward(int questId)
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
    public void UpdateQuestUnlockStates()
    {
        foreach (var progress in ProgressList)
        {
            var meta = GetQuestMeta(progress.QuestID);
            if (progress.Status == QuestStatus.Locked && meta != null && IsQuestUnlockable(meta))
                progress.Status = QuestStatus.Available;
        }
    }

    /// <summary>
    /// 해당 퀘스트가 해금(Available) 조건을 만족하는지 판별합니다.
    /// </summary>
    /// <param name="meta">퀘스트 메타 정보</param>
    /// <returns>해금 가능하면 true</returns>
    private bool IsQuestUnlockable(QuestData meta)
    {
        if (!meta.IsChapterUnlocked(CurrentChapter))
            return false;
        if (meta.PrerequisiteQuestIDs != null && meta.PrerequisiteQuestIDs.Any(id => !IsQuestCompleted(id)))
            return false;
        return true;
    }

    /// <summary>
    /// 퀘스트가 Completed 상태인지 확인합니다.
    /// </summary>
    /// <param name="questId">퀘스트ID</param>
    /// <returns>완료 상태면 true</returns>
    public bool IsQuestCompleted(int questId)
    {
        var progress = GetProgress(questId);
        return progress != null && progress.Status == QuestStatus.Completed;
    }

    /// <summary>
    /// 퀘스트 목표 진행(GoalCount) 증가. 처치/수집 등에서 호출.
    /// </summary>
    /// <param name="questId">퀘스트ID</param>
    /// <param name="amount">증가 수치(기본 1)</param>
    public void AddGoalCount(int questId, int amount = 1)
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
    public void SaveProgress()
    {
        var json = JsonUtility.ToJson(new QuestProgressListWrapper(ProgressList));
        System.IO.File.WriteAllText(SavePath, json);
    }

    /// <summary>
    /// Json 파일에서 퀘스트 진행 정보를 불러옵니다.
    /// </summary>
    public void LoadProgressFromFile()
    {
        if (System.IO.File.Exists(SavePath))
        {
            var json = System.IO.File.ReadAllText(SavePath);
            var wrapper = JsonUtility.FromJson<QuestProgressListWrapper>(json);
            LoadQuestProgress(wrapper.progressList);
        }
    }

    /// <summary>
    /// 퀘스트 진행 저장 파일 경로
    /// </summary>
    private string SavePath => $"{Application.persistentDataPath}/quest_progress.json";

    /// <summary>
    /// Json 저장/로드용 래퍼 클래스
    /// </summary>
    [Serializable]
    private class QuestProgressListWrapper
    {
        public List<QuestProgress> progressList;
        public QuestProgressListWrapper(List<QuestProgress> list) { progressList = list; }
    }
}
