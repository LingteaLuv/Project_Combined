using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class TestGameManager : MonoBehaviour
{
    void Start()
    {
        var questMetaList = new List<QuestData>
        {
            new QuestData
            {
                QuestID = 1002,
                QuestName = "10초 버티기",
                Type = QuestType.Survive,
                Description = "아무것도 하지 말고 10초간 버텨라.",
                UnlockChapters = Chapter.Chapter1,
                SurviveTime = 10f,
                RewardGold = 50,
                RewardItemIDs = new[] { 2010 }
            }
        };
        QuestManager.Instance.LoadQuestMeta(questMetaList);

        var progressList = questMetaList.Select(q => new QuestProgress(q.QuestID)).ToList();
        progressList[0].Status = QuestStatus.Active; // 테스트를 위해 바로 Active
        QuestManager.Instance.LoadQuestProgress(progressList);

        // 자동 실행 예시:
        var quest = QuestManager.Instance.GetQuestMeta(1002);
        var progress = QuestManager.Instance.GetProgress(1002);
        if (quest != null && progress != null && progress.Status == QuestStatus.Active)
            FindObjectOfType<SurviveQuestRunner>().StartSurviveQuest(quest, progress);
    }
}
