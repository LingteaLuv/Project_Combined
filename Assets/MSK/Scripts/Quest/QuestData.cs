using System;

[Serializable]
public class QuestData
{
    public int QuestID;
    public string QuestName;
    public QuestType Type;
    public string Description;
    public Chapter UnlockChapters;
    public int[] PrerequisiteQuestIDs;
    public bool IsMainQuest;

    public int NpcGiverID;
    public int NpcTargetID;

    // 목표 데이터
    public int TargetItemID;
    public int TargetMonsterID;
    public int TargetMonsterCount;
    public float SurviveTime;
    public int TargetLocationID;

    public int RewardGold;
    public int[] RewardItemIDs;

    public bool IsChapterUnlocked(Chapter current) => (UnlockChapters & current) != 0;
}