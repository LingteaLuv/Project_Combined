using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private List<DialogueSO> _dialogues;
    [SerializeField] private List<NPCSO> _npc;
    
    [SerializeField] private TextMeshProUGUI _scriptScreen;
    [SerializeField] private TextMeshProUGUI _npcName;
    
    private WaitForSeconds _delay;
    private NPCDialogue _curNPC;
    
    public Dictionary<int, DialogueSO> DialogueDic { get; private set; }
    public Dictionary<int, NPCSO> NPCDic { get; private set; }
    
    private void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        DialogueDic = new Dictionary<int, DialogueSO>();
        NPCDic = new Dictionary<int, NPCSO>();
        for (int i = 0; i < _dialogues.Count; i++)
        {
            DialogueDic.Add(_dialogues[i].DialogueID, _dialogues[i]);
        }
        for (int i = 0; i < _npc.Count; i++)
        {
            NPCDic.Add(_npc[i].NPCID, _npc[i]);
        }
        _delay = new WaitForSeconds(0.05f);
    }

    /// <summary>
    /// PlayerInteractState에서 NPC를 감지하고 F키를 누르면 호출되는 함수
    /// </summary>
    /// <param name="npc">상호작용하는 npc</param>
    public void SetDialogue(NPCDialogue npc)
    {
        // 현재 대화중인 NPC(_curNPC) 감지한 NPC로 설정
        _curNPC = npc;
        // 해당 NPC의 현재 대사를 출력하는 코루틴 호출
        StartCoroutine(PrintOut());
    }

    public Dictionary<int, int> GetDialogueFlow(int id)
    {
        Dictionary<int, int> dic = new Dictionary<int, int>();
        foreach (var dialogue in DialogueDic.Values)
        {
            if (dialogue.NPCID == id)
            {
                dic.Add(dialogue.DialogueID,dialogue.LoofDialogueID);
            }
        }
        return dic;
    }
    
    private IEnumerator PrintOut()
    {
        // NPC가 담당하는 퀘스트를 확인하여 관련 대사를 출력해야하는지 확인 
        _curNPC.CheckQuest(QuestManager.Instance.QuestDictionary);
        
        // 시작 대사 ID를 현재 대사로 설정 
        int startId = _curNPC.CurrentDialogueID;
        _curNPC.CheckLoop(startId);
        // startID를 DialogueDic가 가지는지 확인하고, 해당 Dialogue가 마지막일 때까지
        while (DialogueDic.ContainsKey(startId) && DialogueDic[startId].EndCheck)
        {
            _npcName.text = NPCDic[DialogueDic[startId].NPCID].Name;
            
            // Dialogue 한글자씩 출력, F를 누르면 대사 한 번에 보이도록(스킵) 구현
            yield return ScriptSetting.WriteWords(_scriptScreen, DialogueDic[startId].DialogueText, _delay, () => SkipRequested());
            
            // 대사가 연속되니까 startID에 1을 더함
            startId++;
            
            // 플레이어 입력까지 대기(F)
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
        }
    }

    private bool SkipRequested()
    {
        return Input.GetKeyDown(KeyCode.F);
    }
}
