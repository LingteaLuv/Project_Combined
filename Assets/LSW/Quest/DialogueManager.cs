using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private List<DialogueSO> _dialogues;
    [SerializeField] private List<DialogueChoiceSO> _choicesDialogues;
    [SerializeField] private TextMeshProUGUI _scriptScreen;

    [SerializeField] public Button Button1;
    [SerializeField] public Button Button2;
    [SerializeField] public Button Button3;

    //  해당 대사를 쳤는가?
    private int _selectedNextId = -1;
    [SerializeField] private List<NPCSO> _npc;
    [SerializeField] private TextMeshProUGUI _npcName;
    
    private WaitForSeconds _delay;
    private NPCDialogue _curNPC;
    
    public Dictionary<int, DialogueSO> DialogueDic { get; private set; }
    public Dictionary<string, DialogueChoiceSO> ChoiceDic { get; private set; }

    public Dictionary<string, NPCSO> NPCDic { get; private set; }
    
    private void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        DialogueDic = new Dictionary<int, DialogueSO>();
        NPCDic = new Dictionary<string, NPCSO>();
        for (int i = 0; i < _dialogues.Count; i++)
        {
            DialogueDic.Add(_dialogues[i].DialogueID, _dialogues[i]);
        }
        for (int i = 0; i < _npc.Count; i++)
        {
            NPCDic.Add(_npc[i].NPCID, _npc[i]);
        }
        _delay = new WaitForSeconds(0.05f);

        ChoiceDic = new Dictionary<string, DialogueChoiceSO>();
        foreach (var c in _choicesDialogues)
            ChoiceDic.Add(c.DialogueChoiceID, c);
        HideAllButtons();
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

    public Dictionary<int, int> GetDialogueFlow(string id)
    {
        Dictionary<int, int> dic = new Dictionary<int, int>();
        foreach (var dialogue in DialogueDic.Values)
        {
            if (dialogue.NPCID == id && dialogue.LoopDialogueID != 0)
            {
                dic.Add(dialogue.DialogueID,dialogue.LoopDialogueID);
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
        
        // startID를 DialogueDic가 가지는지 확인하고, 해당 Dialogue가 마지막일 때까지
        while (DialogueDic.ContainsKey(startId))
        {
            _npcName.text = NPCDic[DialogueDic[startId].NPCID].Name;
            
            // Dialogue 한글자씩 출력, F를 누르면 대사 한 번에 보이도록(스킵) 구현
            yield return ScriptSetting.WriteWords(_scriptScreen, DialogueDic[startId].DialogueText, _delay, () => SkipRequested());
            
            // 다음 대사 ID 변경
            if (!String.IsNullOrEmpty(DialogueDic[startId].DialogueChoiceID))
            {
                DialogueChoiceSO choice = ChoiceDic[DialogueDic[startId].DialogueChoiceID];
                ShowChoiceButtons(choice);
                _selectedNextId = -1;
                yield return new WaitUntil(() => _selectedNextId != -1);
                startId = _selectedNextId;
            }
            else
            {
                if (DialogueDic[startId].EndCheck)
                {
                    _curNPC.CheckLoop(startId);
                    break;
                }
                else
                {
                    startId++;
                }
                // 플레이어 입력까지 대기(F)
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
            }
        }
    }

    private bool SkipRequested()
    {
        return Input.GetKeyDown(KeyCode.F);
    }

    /// <summary>
    /// 대사와 선택지를 출력하고 분기 처리용 오버로드 함수
    /// </summary>
    /// <param name="dialogueId">출력할 대사의 ID</param>
    private IEnumerator PrintOut(int dialogueId)
    {
        DialogueSO dialogue = DialogueDic[dialogueId];
        yield return ScriptSetting.WriteWords(_scriptScreen, dialogue.DialogueText, new WaitForSeconds(0.05f), () => SkipRequested());

        if (!String.IsNullOrEmpty(dialogue.DialogueChoiceID))
        {
            DialogueChoiceSO choice = ChoiceDic[dialogue.DialogueChoiceID];
            ShowChoiceButtons(choice);

            _selectedNextId = -1;
            
            yield return new WaitUntil(() => _selectedNextId != -1);

            yield return StartCoroutine(PrintOut(_selectedNextId));
            yield break;
        }

        if (dialogue.EndCheck)
        {
            yield return StartCoroutine(PrintOut(dialogueId + 1));
        }
        // else: 대화 종료
    }

    /// <summary>
    /// 선택지 버튼 UI를 한 번에 세팅한다.
    /// </summary>
    /// <param name="choice">선택지 ScriptableObject</param>
    private void ShowChoiceButtons(DialogueChoiceSO choice)
    {
        SetChoiceButton(Button1, choice.Number1, choice.NextDialogue1ID);
        SetChoiceButton(Button2, choice.Number2, choice.NextDialogue2ID);
        SetChoiceButton(Button3, choice.Number3, choice.NextDialogue3ID);
    }

    /// <summary>
    /// 선택지 버튼을 숨긴다.
    /// </summary>
    private void HideAllButtons()
    {
        Button1.gameObject.SetActive(false);
        Button2.gameObject.SetActive(false);
        Button3.gameObject.SetActive(false);
    }
    /// <summary>
    /// 개별 버튼의 텍스트와 클릭 이벤트를 설정한다.
    /// </summary>
    /// <param name="btn">버튼</param>
    /// <param name="label">텍스트</param>
    /// <param name="nextDialogueId">분기될 다음 대사 ID</param>
    private void SetChoiceButton(Button btn, string label, int nextDialogueId)
    {
        if (!string.IsNullOrEmpty(label))
        {
            btn.gameObject.SetActive(true);
            btn.GetComponentInChildren<TMP_Text>().text = label;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                _selectedNextId = nextDialogueId;
                HideAllButtons();
            });
        }
        else
        {
            btn.gameObject.SetActive(false);
        }
    }

    public void StartDialogueFromId(int dialogueId)
    {
        StartCoroutine(PrintOut(dialogueId));
    }

}
