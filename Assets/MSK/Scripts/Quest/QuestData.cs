using UnityEngine;

/// <summary>
/// 퀘스트 데이터(QuestManager에서 관리). 
/// 챕터, 타입, 선행퀘, 목표, 보상 등 모든 속성 포함
/// </summary>
[CreateAssetMenu(menuName = "Quest")]
public class QuestData :ScriptableObject
{
    public string QuestID;                          // 고유 퀘스트 ID
    public QuestType Type;                       // 퀘스트 타입(대화/전달/회수)
    public QuestStatus Status;                   // 상태: Locked, Available, Active, Completed, Closed
    public string Description;                   // 퀘스트 설명(생존일지에 작성될 내용)
    
    public string StartNPCID;                    // 퀘스트 제공(시작) NPC ID  
    public int StartDialogueID;               // 시작 NPC 퀘스트 대사
    public string EndNPCID;                      // 퀘스트 완료(종료) NPC ID(대화,회수)
    public int EndDialogueID;                 // 종료 NPC 퀘스트 대사  
    
    public string RequiredItemID;                // (전달,회수) 목표 아이템 ID
    public int RequiredItemQuantity;          // (전달,회수) 목표 아이템 수량
    
    public string RewardItemID;                  // 보상 아이템 ID
    public int RewardItemQuantity;            // 보상 아이템 수량
    
    public string NextQuestID;                   // 다음 연계 퀘스트 ID
    public string EndDescription;

    public string DeliveryNpcID;
    public string TriggerID1;
    public string TriggerID2;
}