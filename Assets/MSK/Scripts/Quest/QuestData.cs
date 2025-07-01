/// <summary>
/// 퀘스트 데이터(QuestManager에서 관리). 
/// 챕터, 타입, 선행퀘, 목표, 보상 등 모든 속성 포함
/// </summary>
public class QuestData
{
    public int QuestID;                           // 고유 퀘스트 ID
    public string QuestName;                      // 퀘스트 이름
    public QuestType Type;                        // 퀘스트 타입(대화/전달/처치 등, 비트플래그)
    public string Description;                    // 퀘스트 설명

    public Chapter UnlockChapters;                // 해금 가능한 챕터(비트플래그)
    public int[] PrerequisiteQuestIDs;            // 선행 퀘스트 ID(여러 개 가능)
    public bool IsMainQuest;                      // 메인/서브 구분

    public QuestStatus Status;                    // 상태: Locked, Available, Active, Completed, Closed

    public int NpcGiverID;                        // 퀘스트 제공(시작) NPC ID
    public int NpcTargetID;                       // 타겟 NPC ID(필요시, ex: 전달/대화 등)

    // 목표(Goal) - 타입에 따라 구조적 분기
    public int TargetItemID;                      // (전달) 목표 아이템 ID
    public int TargetMonsterID;                   // (처치) 목표 몬스터 ID
    public int TargetMonsterCount;                // (처치) 목표 처치 수
    public float SurviveTime;                     // (버티기) 목표 시간(초)
    public int TargetLocationID;                  // (도달) 목표 위치ID(또는 좌표/타겟ID 등)

    // 보상
    public int RewardGold;
    public int[] RewardItemIDs;                   // 보상 아이템(복수 가능)

    /// <summary>
    /// 선행 퀘스트가 하나라도 있는지 여부를 반환합니다.
    /// </summary>
    public bool HasPrerequisite => PrerequisiteQuestIDs != null && PrerequisiteQuestIDs.Length > 0;

    /// <summary>
    /// 현재 챕터(current)에서 이 퀘스트가 해금(진행 가능) 상태인지 확인합니다.
    /// </summary>
    /// <param name="current">플레이어의 현재 챕터(Chapter 비트 플래그)</param>
    /// <returns>진행 가능하면 true, 아니면 false</returns>
    public bool IsChapterUnlocked(Chapter current)
        => (UnlockChapters & current) != 0;
}