using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private List<DialogueSO> _dialogues;
    [SerializeField] private List<DialogueChoiceSO> _choicesDialogues;
    [SerializeField] private List<NPCSO> _npc;
    
    [SerializeField] private TextMeshProUGUI _scriptScreen;
    [SerializeField] private GameObject _dialogueCanvas;

    [SerializeField] private GameObject _button1Obj;
    [SerializeField] private GameObject _button2Obj;
    [SerializeField] private GameObject _button3Obj;

    private Button Button1;
    private Button Button2;
    private Button Button3;
    private TMP_Text Button1Text;
    private TMP_Text Button2Text;
    private TMP_Text Button3Text;

    public event Action OffDialogue;

    //  버튼 입력 전까지 대기상태
    private int _selectedNextId = -1;
    [SerializeField] private TextMeshProUGUI _npcName;

    private Coroutine _dialogueCoroutine;
    private WaitForSeconds _delay;
    private NPCDialogue _curNPC;
    
    public Dictionary<int, DialogueSO> DialogueDic { get; private set; }
    public Dictionary<string, DialogueChoiceSO> ChoiceDic { get; private set; }

    public Dictionary<string, bool> TriggerDic { get; private set; }
    public Dictionary<string, NPCSO> NPCDic { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    
    
    private void Init()
    {
        DialogueDic = new Dictionary<int, DialogueSO>();
        NPCDic = new Dictionary<string, NPCSO>();
        TriggerDic = new Dictionary<string, bool>();
        for (int i = 0; i < _dialogues.Count; i++)
        {
            DialogueDic.Add(_dialogues[i].DialogueID, _dialogues[i]);
        }
        for (int i = 0; i < _npc.Count; i++)
        {
            NPCDic.Add(_npc[i].NPCID, _npc[i]);
        }

        for (int i = 0; i < _dialogues.Count; i++)
        {
            if (!TriggerDic.ContainsKey(_dialogues[i].TriggerID))
            {
                TriggerDic.Add(_dialogues[i].TriggerID, false);
            }
        }
        
        _delay = new WaitForSeconds(0.05f);

        ChoiceDic = new Dictionary<string, DialogueChoiceSO>();
        foreach (var c in _choicesDialogues)
            ChoiceDic.Add(c.DialogueChoiceID, c);

        Button1 = _button1Obj.GetComponentInChildren<Button>(true);
        Button2 = _button2Obj.GetComponentInChildren<Button>(true);
        Button3 = _button3Obj.GetComponentInChildren<Button>(true);

        Button1Text = _button1Obj.GetComponentInChildren<TMP_Text>(true);
        Button2Text = _button2Obj.GetComponentInChildren<TMP_Text>(true);
        Button3Text = _button3Obj.GetComponentInChildren<TMP_Text>(true);
        HideAllButtons();
        _dialogueCanvas.SetActive(false);

        QuestManager.Instance.OnQuestAccepted += AcceptTrigger;
        QuestManager.Instance.OnQuestClosed += ClearTrigger;
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
        if (_dialogueCoroutine == null)
        {
            _dialogueCoroutine = StartCoroutine(PrintOut());
        }
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
        _dialogueCanvas.SetActive(true);
        // NPC가 담당하는 퀘스트를 확인하여 관련 대사를 출력해야하는지 확인 
        _curNPC.CheckDialogue(QuestManager.Instance.QuestDictionary);
        // 시작 대사 ID를 현재 대사로 설정 
        int startId = _curNPC.CurrentDialogueID;

        List<int> idCache = new List<int>();
        
        // startID를 DialogueDic가 가지는지 확인하고, 해당 Dialogue가 마지막일 때까지
        while (DialogueDic.ContainsKey(startId))
        {
            //yield return new WaitWhile(() => Input.GetKey(KeyCode.F));
            
            _npcName.text = NPCDic[DialogueDic[startId].NPCID].Name;
            idCache.Add(startId);
            // Dialogue 한글자씩 출력, F를 누르면 대사 한 번에 보이도록(스킵) 구현
            yield return ScriptSetting.WriteWords(_scriptScreen, DialogueDic[startId].DialogueText, _delay, () => SkipRequested());
            
            if (DialogueDic[startId].TriggerID != null)
                QuestManager.Instance.SetQuestType(DialogueDic[startId].TriggerID);
            
            // 다음 대사 ID 변경s
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
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
        
        _dialogueCanvas.SetActive(false);
        _dialogueCoroutine = null;
        OffDialogue?.Invoke();
    }

    private void AcceptTrigger(QuestData quest, QuestProgress progress)
    {
        TriggerDic[quest.TriggerID1] = true;
    }
    
    private void ClearTrigger(QuestData quest, QuestProgress progress)
    {
        TriggerDic[quest.TriggerID2] = true;
    }
    
    private bool SkipRequested()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }
    
    /// <summary>
    /// 대사와 선택지를 출력하고 분기 처리용 오버로드 함수
    /// </summary>
    /// <param name="startId">출력할 대사의 ID</param>
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
    }

    /// <summary>
    /// 선택지 버튼 UI를 한 번에 세팅한다.
    /// </summary>
    /// <param name="choice">선택지 ScriptableObject</param>
    private void ShowChoiceButtons(DialogueChoiceSO choice)
    {
        SetChoiceButton(_button1Obj, Button1, Button1Text, choice.Number1, choice.NextDialogue1ID);
        SetChoiceButton(_button2Obj, Button2, Button2Text, choice.Number2, choice.NextDialogue2ID);
        SetChoiceButton(_button3Obj, Button3, Button3Text, choice.Number3, choice.NextDialogue3ID);
    }

    /// <summary>
    /// 선택지 버튼을 숨긴다.
    /// </summary>
    private void HideAllButtons()
    {
        _button1Obj.SetActive(false);
        _button2Obj.SetActive(false);
        _button3Obj.SetActive(false);
        _button1Obj.transform.parent.gameObject.SetActive(true);
    }
    /// <summary>
    /// 개별 버튼의 텍스트와 클릭 이벤트를 설정한다.
    /// </summary>
    /// <param name="btn">버튼</param>
    /// <param name="label">텍스트</param>
    /// <param name="nextDialogueId">분기될 다음 대사 ID</param>
    private void SetChoiceButton(GameObject buttonObj, Button btn, TMP_Text btnText, string label, int nextDialogueId)
    {
        if (!string.IsNullOrEmpty(label))
        {
            buttonObj.SetActive(true);  // 부모 오브젝트 활성화
            btnText.text = label;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                _selectedNextId = nextDialogueId;
                HideAllButtons();
            });
        }
        else
        {
            buttonObj.SetActive(false); // 부모 오브젝트 비활성화
        }
    }

    public void StartDialogueFromId(int dialogueId)
    {
        StartCoroutine(PrintOut(dialogueId));
    }
}
