using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField]
    private List<DialogueSO> _dialogues;
    
    private Dictionary<string, DialogueSO> _dialogueDic;
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        
    }

    private void Init()
    {
        _dialogueDic = new Dictionary<string, DialogueSO>();

        for (int i = 0; i < _dialogues.Count; i++)
        {
            _dialogueDic.Add(_dialogues[i].DialogueID, _dialogues[i]);
        }
    }

    public DialogueSO GetDialogue(string id)
    {
        return _dialogueDic[id];
    }
}
