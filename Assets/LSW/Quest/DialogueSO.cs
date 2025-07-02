using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue")]
public class DialogueSO : ScriptableObject
{
    // 각 대사 ID
    public int DialogueID;
    // 대사 출력 NPC, Player일 경우 주인공 독백 처리 => 공백
    public int NPCID;
    // 대사 내용
    public string DialogueText;
    // 선택지 묶음 ID
    public int DialogueChoiceID;
    // 대화가 한 번 진행되면 진행되는 dialogue
    public int LoofDialogueID;
    public bool EndCheck;
    public string TriggerID;
}
