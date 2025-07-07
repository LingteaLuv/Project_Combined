using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class NPCInteractable : MonoBehaviour
{
    [SerializeField] public NPCDialogue Dialogue;

    [SerializeField] public NPCMark Mark;

    [SerializeField] private BallonRotate _ballon;

    public bool IsInteractable;

    public string ID => Dialogue._data.NPCID;

    private void Awake()
    {
        IsInteractable = true;
    }
    private void Start()
    {
        Mark.gameObject.transform.rotation = Quaternion.Euler(90, 90, 0);
        Mark.SetNone();
        _ballon.SetNone();
        _ballon.gameObject.SetActive(false);

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
                _ballon.SetAvailable();
                Mark.SetAvailable();
                break;
            }
        }
    }


    private void SetAvailableText(QuestData qd, QuestProgress qp)
    {
        if (qd.StartNPCID == ID)
        {
            _ballon.SetAvailable();
        }
    }
    private void SetAcceptedText(QuestData qd, QuestProgress qp)
    {
        if (qd.StartNPCID == ID)
        {
            _ballon.SetNone();
            Mark.SetNone();
        }
    }
    private void SetCompletedText(QuestData qd, QuestProgress qp)
    {
        if (qd.EndNPCID == ID)
        {
            _ballon.SetCompleted();
            Mark.SetCompleted();
        }
    }
    private void SetClosedText(QuestData qd, QuestProgress qp)
    {
        if (qd.EndNPCID == ID)
        {
            _ballon.SetNone();
            Mark.SetNone();
        }
    }


    public void OnBallon()
    {
        _ballon.gameObject.SetActive(true);
    }
    public void OffBallon()
    {
        _ballon.gameObject.SetActive(false);
    }


}
