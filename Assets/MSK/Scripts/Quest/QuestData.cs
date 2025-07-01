using UnityEngine;

/// <summary>
/// 퀘스트 데이터(QuestManager에서 관리). 
/// 챕터, 타입, 선행퀘, 목표, 보상 등 모든 속성 포함
/// </summary>
public class QuestData :ScriptableObject
{
    public string QuestID;                        // 고유 퀘스트 ID// 퀘스트 이름
    public QuestType Type;                        // 퀘스트 타입(대화/전달/처치 등, 비트플래그)
    public string Description;                    // 퀘스트 설명
    public string OnTriggerID;
    
    public QuestStatus Status;                    // 상태: Locked, Available, Active, Completed, Closed

    public int StartNPCID;   
    public string StartDialogueID;                // 퀘스트 제공(시작) NPC ID

    public int EndNPCID;       
    public string EndDialogueID;                  // 타겟 NPC ID(필요시, ex: 전달/대화 등)

    public string NextQuestID;
    // 목표(Goal) - 타입에 따라 구조적 분기
    public string RequiredItemID;                 // (전달) 목표 아이템 ID
    public string RequiredItemQuantity; 
    
    public int TargetMonsterID;                   // (처치) 목표 몬스터 ID
    public int TargetMonsterCount;                // (처치) 목표 처치 
    public int GoalCount;

    public int TargetLocationID;                  // (도달) 목표 위치ID(또는 좌표/타겟ID 등)

    // 보상
    public string RewardItemID;
    public string RewardItemQuantity;
}