using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItemGen : MonoBehaviour
{
    [SerializeField] GameObject _prefab;

    [SerializeField] NPCDialogue _dialogue;

    [SerializeField] Vector3 _pos;

    private void Awake()
    {
        _dialogue = GetComponentInParent<NPCDialogue>();
    }

    private void Start()
    {
        QuestManager.Instance.OnQuestAccepted += ItemGen;
    }

    private void ItemGen(QuestData q, QuestProgress qp)
    {
        if (q.StartNPCID == _dialogue._data.NPCID)
        {
            Instantiate(_prefab, _pos, Quaternion.identity);
        }
    }
}
