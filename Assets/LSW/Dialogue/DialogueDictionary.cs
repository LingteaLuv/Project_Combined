using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dictionary/DialogueDictionary")]
public class DialogueDictionary : ScriptableObject
{
    [SerializeField] private List<DialogueSO> Dialogues;
    public Dictionary<int, DialogueSO> DialogueDic { get; private set; }
    
    public void GenerateDic()
    {
        Init();
    }

    private void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        DialogueDic = new Dictionary<int, DialogueSO>();
        
        for (int i = 0; i < Dialogues.Count; i++)
        {
            int key = Dialogues[i].DialogueID;
            DialogueDic.Add(key,Dialogues[i]);
        }
    }
}
