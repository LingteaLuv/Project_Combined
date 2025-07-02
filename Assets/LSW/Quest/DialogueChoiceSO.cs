using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "DialogueChoice")]
public class DialogueChoiceSO : ScriptableObject
{
    // 선택지 그룹 ID
    public int DialogueChoiceID;
    // 선택지 대사
    public string Number1;
    // 선택지 선택시 다음 대사
    public int NextDialogue1ID;
    public string Number2;
    public int NextDialogue2ID;
    public string Number3;
    public int NextDialogue3ID;
}
