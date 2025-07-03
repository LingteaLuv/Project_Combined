using System;

[Serializable]
public class QuestProgress
{
    public string QuestID;
    public QuestStatus Status;
    public int GoalCount;
    public bool IsRewardClaimed;

    public QuestProgress(string questID)
    {
        QuestID = questID;
        Status = QuestStatus.Locked;
        GoalCount = 0;
        IsRewardClaimed = false;
    }
}