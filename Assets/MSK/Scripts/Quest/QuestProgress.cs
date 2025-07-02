using System;

[Serializable]
public class QuestProgress
{
    public int QuestID;
    public QuestStatus Status;
    public int GoalCount;
    public bool IsRewardClaimed;

    public QuestProgress(int questID)
    {
        QuestID = questID;
        Status = QuestStatus.Locked;
        GoalCount = 0;
        IsRewardClaimed = false;
    }
}