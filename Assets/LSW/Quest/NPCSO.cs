using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC")]
public class NPCSO : ScriptableObject
{
    public string NPCID;

    // 등장조건 
    public string NPCTriggerID;
    public string BasicDialogueID;

    public string Trigger1;
    public string Trigger1Dialogue;
    public string Trigger2;
    public string Trigger2Dialogue;
    public string Trigger3;
    public string Trigger3Dialogue;

    public int StartQusetID;
    public int TriggerQuestID;
    public int QuestTriggerID;

    public int StartDialogueID;
    public int EndDialogueID;
}
