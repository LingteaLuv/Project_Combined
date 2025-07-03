using UnityEngine;

public class QuestTestRunner : MonoBehaviour
{
    public string testTriggerId; // 인스펙터에서 입력

    void Start()
    {
        Debug.Log("QuestType() 실행됨! 트리거ID: " + testTriggerId);
        // (선택) 퀘스트 상태 변화 이벤트 구독
        QuestManager.Instance.OnQuestAccepted += (data, progress) => Debug.Log("수주: " + data.QuestID);
        QuestManager.Instance.OnQuestCompleted += (data, progress) => Debug.Log("완료: " + data.QuestID);
        QuestManager.Instance.OnQuestClosed += (data, progress) => Debug.Log("종료: " + data.QuestID);

        // 테스트: 원하는 triggerId로 QuestType 호출
        Debug.Log("테스트 시작! triggerId: " + testTriggerId);
        QuestManager.Instance.SetQuestType(testTriggerId);

        // 상태 확인 (디버그용)
        foreach (var quest in QuestManager.Instance.QuestDictionary.Values)
        {
            Debug.Log($"퀘스트 {quest.QuestID} 상태: {quest.Status}");
        }
    }
}
