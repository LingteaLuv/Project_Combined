using System;

[Serializable]
public class QuestProgress
{
    public string QuestID;
    public QuestStatus Status;
    public bool IsRewardClaimed;

    public QuestProgress(string questID)
    {
        QuestID = questID;
        Status = QuestStatus.Locked;
        IsRewardClaimed = false;
    }
}