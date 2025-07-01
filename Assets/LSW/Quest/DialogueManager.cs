using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField]
    private List<DialogueSO> _dialogues;

    private NPCDialogue _curNPC;
    
    public Dictionary<int, DialogueSO> DialogueDic { get; private set; }
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        
    }

    private void Init()
    {
        DialogueDic = new Dictionary<int, DialogueSO>();

        for (int i = 0; i < _dialogues.Count; i++)
        {
            DialogueDic.Add(_dialogues[i].DialogueID, _dialogues[i]);
        }
    }

    public DialogueSO GetDialogue(int id)
    {
        return DialogueDic[id];
    }

    public void SetDialogue(NPCDialogue npc)
    {
        _curNPC = npc;
        PrintOut();
    }

    private void PrintOut()
    {
        int startId = _curNPC.Data.StartDialogueID;
        while (DialogueDic[startId].EndCheck)
        {
            //ScriptSetting.WriteWords()
        }
    }
}
