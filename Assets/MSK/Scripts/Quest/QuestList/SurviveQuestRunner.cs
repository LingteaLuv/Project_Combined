using UnityEngine;
using System.Collections;

public class SurviveQuestRunner : MonoBehaviour
{
    public void StartSurviveQuest(QuestData quest, QuestProgress progress)
    {
        StartCoroutine(RunSurviveQuest(quest, progress));
    }

    private IEnumerator RunSurviveQuest(QuestData quest, QuestProgress progress)
    {
        float timer = 0f;
        Debug.Log($"[퀘스트 시작] {quest.QuestName} ({quest.SurviveTime}초 버티기)");

        while (timer < quest.SurviveTime)
        {
            timer += Time.deltaTime;
            progress.GoalCount = Mathf.CeilToInt(timer);
            Debug.Log($"진행 상황: {progress.GoalCount} / {quest.SurviveTime}초");
            yield return null;
        }
        Debug.Log($"[퀘스트 완료] {quest.QuestName} - {quest.SurviveTime}초 버티기 성공!");
        QuestManager.Instance.CompleteQuest(quest.QuestID);
    }
}