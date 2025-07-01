/// <summary>
/// 퀘스트의 진행 상태를 정의하는 열거형입니다.
/// </summary>
public enum QuestStatus
{
    Locked,        // 잠긴 상태(미해금/수락 불가)
    Available,     // 진행 가능(해금 완료, 수락 대기)
    Active,        // 진행 중(플레이어가 수락함)
    Completed,     // 완료(모든 목표 달성)
    Rewarded,      // 보상 수령 완료(완료 후 보상까지 받은 상태, 선택)
    Failed,        // 실패(조건 미달, 시간 초과 등, 선택)
    Closed         // 종료/아카이브(더 이상 재진행 불가, 선택)
}