using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC")]
public class NPCSO : ScriptableObject
{
    public string NPCID;

    public string Name;
    
    // 등장조건 
    public int BasicDialogueID;

    public string Trigger1;
    public int Trigger1DialogueID;
    public string Trigger2;
    public int Trigger2DialogueID;
    public string Trigger3;
    public int Trigger3DialogueID;
    public int StartQuestID;
}
