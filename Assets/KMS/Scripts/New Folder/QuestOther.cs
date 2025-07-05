using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestOther : MonoBehaviour
{

    [SerializeField] NPCInteractable _ni;

    private void Awake()
    {
        _ni = GetComponentInParent<NPCInteractable>();
    }

    private void Start()
    {
        DialogueManager.Instance.OnTimeline += PostBox; //우체통 관련

        QuestManager.Instance.OnQuestAccepted += PostBoxInteractable;


    }

    private void PostBox(string id)
    {
        if (id == "우체통")
        {
            InventoryManager.Instance.FindItemByID(3010);
        }
    }

    private void PostBoxInteractable(QuestData q, QuestProgress qp)
    {
        if (q.QuestID == "Civil02")
        {
            _ni.IsInteractable = true;
        }
    }
}
