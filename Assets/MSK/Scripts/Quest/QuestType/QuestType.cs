/// <summary>
/// 퀘스트의 종류를 비트 플래그로 정의한 열거형입니다.
/// 여러 타입을 동시에 지정할 수 있습니다.
/// </summary>
[System.Flags]
public enum QuestType
{
    Talk     = 0,           // NPC와의 대화(또는 튜토리얼용)
    Delivery = 1 << 1,  // 아이템 전달/전달 퀘스트
    Collect     = 1 << 2,      // 몬스터 처치 퀘스트
    Survive  = 1 << 3,   // 시간 버티기 퀘스트
    Reach    = 1 << 4,     // 특정 위치 도달 퀘스트
    Ending = 1 << 5,        // 엔딩인가요?
    // 필요시 추가 타입 확장
}

//  QuestType type = QuestType.Kill | QuestType.Delivery; // 0100 | 0010 = 0110

//  예: Delivery=2, Kill=4, Survive=8
//  type = Delivery | Kill | Survive = 2 | 4 | 8 = 14 (2진수로 1110)
//  QuestType type = QuestType.Delivery | QuestType.Kill | QuestType.Survive; // 14