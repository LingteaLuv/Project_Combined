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

    public string StartQusetID;
    public string TriggerQuestID;
    public string QuestTriggerID;

    public string StartDialogueID;
    public string EndDialogueID;
}
