using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue : MonoBehaviour
{
    [Tooltip("Npc의 이름")]
    public string npcName;          // npc 이름

    [Tooltip("대사의 내용")]
    public TextAsset npcQuestText;  // npc에 할당된 텍스트
  
}

[System.Serializable]
public class DialogueEvent
{
    public string eventName;
    public Vector2 line;
    public Dialogue[] dialogues;
}
