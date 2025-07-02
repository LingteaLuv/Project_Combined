using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestContentControl : MonoBehaviour
{
    [SerializeField] private GameObject _questNode;

    public Dictionary<string, QuestNode> NodeDict;
    void Start()
    {
        //퀘스트 목록을 순회한다. 목록을 순회하면서 게임오브젝트를 생성하면서 그 게임오브젝트와 아이디를 nodedict에 처넣는다.
        //생성 후 디스크립션 바로 적용, 이후 비활성화
        
        foreach (KeyValuePair<string, QuestData> q in QuestManager.Instance.QuestDictionary)
        {
            GameObject go = Instantiate(_questNode, gameObject.transform);
            NodeDict.Add(q.Key, go.GetComponent<QuestNode>());
            NodeDict[q.Key].Description.text = q.Value.Description;
            NodeDict[q.Key].EndDescription.text = q.Value.EndDescription;
            NodeDict[q.Key].EndDescription.enabled = false;
            go.SetActive(false);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        QuestManager.Instance.OnQuestAccepted += ActiveNode;
        //QuestManager.Instance.OnQuestCompleted += CheckNode;
        
    }
    public void ActiveNode(QuestData q, QuestProgress qp)
    {
        //아이디와 일치하는 노드 활성화
        NodeDict[q.QuestID].gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
    public void CheckNode(QuestData q, QuestProgress qp)
    {
        //아이디와 일치하는 노드 활성화
        NodeDict[q.QuestID].Description.fontStyle = TMPro.FontStyles.Strikethrough | TMPro.FontStyles.Bold;
        NodeDict[q.QuestID].EndDescription.enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
}
