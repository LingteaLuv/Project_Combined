using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class NPCInteractable : MonoBehaviour
{
    [SerializeField] public NPCDialogue Dialogue;

    [SerializeField] private GameObject _ballon;
    [SerializeField] private TMP_Text _ballonText;

    public bool IsInteractable;

    public string ID => Dialogue._data.NPCID;

    private void Awake()
    {
        IsInteractable = true;
    }
    private void Start()
    {
        _ballon.SetActive(false);

        QuestManager.Instance.OnQuestAccepted += SetAcceptedText;
        QuestManager.Instance.OnQuestCompleted += SetCompletedText;
        QuestManager.Instance.OnQuestClosed += SetClosedText;
        //시작했을 때, _startquests를 돌면서 Available한게 있으면 ? 로 한다.

        StartCoroutine(Delay());

        if (Dialogue._data.BasicDialogueID == 0) //우체통 관련
        {
            IsInteractable = false;
        }

    }

    private IEnumerator Delay()
    {
        yield return new WaitForEndOfFrame();
        foreach (string s in Dialogue._startQuest)
        {
            if (QuestManager.Instance.QuestDictionary[s].Status == QuestStatus.Available)
            {
                _ballonText.text = "?";
                break;
            }
        }
    }


    private void SetAvailableText(QuestData qd, QuestProgress qp)
    {
        if (qd.StartNPCID == ID)
        {
            _ballonText.text = "?";
        }
    }
    private void SetAcceptedText(QuestData qd, QuestProgress qp)
    {
        if (qd.StartNPCID == ID)
        {
            _ballonText.text = "...";
        }
    }
    private void SetCompletedText(QuestData qd, QuestProgress qp)
    {
        if (qd.EndNPCID == ID)
        {
            _ballonText.text = "!";
        }
    }
    private void SetClosedText(QuestData qd, QuestProgress qp)
    {
        if (qd.EndNPCID == ID)
        {
            _ballonText.text = "...";
        }
    }


    public void OnBallon()
    {
        _ballon.SetActive(true);
    }
    public void OffBallon()
    {
        _ballon.SetActive(false);
    }


}
