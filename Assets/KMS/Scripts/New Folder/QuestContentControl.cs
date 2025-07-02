using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestContentControl : MonoBehaviour
{
    [SerializeField] private GameObject _questNode;

    public Dictionary<int, QuestNode> NodeDict;
    void Start()
    {
        //퀘스트 목록을 순회한다. 목록을 순회하면서 게임오브젝트를 생성하면서 그 게임오브젝트와 아이디를 nodedict에 처넣는다.
        //생성 후 디스크립션 바로 적용, 이후 비활성화
        //Dictionary<int, QuestData> quests = new(); //예시
        //
        //foreach (KeyValuePair<int, QuestData> q in quests)
        //{
        //    GameObject go = Instantiate(_questNode, gameObject.transform);
        //    go.SetActive(false);
        //    NodeDict.Add(q.Key, go.GetComponent<QuestNode>());
        //}
        //
        //QuestManager.Instance.OnQuestAccepted += ActiveNode;
        //QuestManager.Instance.OnQuestCompleted += CheckNode;
        
    }
    public void ActiveNode(QuestData q)
    {
        //아이디와 일치하는 노드 활성화
        NodeDict[q.QuestID].gameObject.SetActive(true);
    }
    public void CheckNode(QuestData q)
    {
        //아이디와 일치하는 노드 활성화
        NodeDict[q.QuestID].Description.fontStyle = TMPro.FontStyles.Strikethrough;
    }
    public void ClearNode(int ID)
    {
        //아이디와 일치하는 노드의 글씨에 선을 긋는다.
        NodeDict[ID].Description.fontStyle = TMPro.FontStyles.Strikethrough;
    }
    public void ActiveNode(int ID)
    {
        //아이디와 일치하는 노드 활성화
        NodeDict[ID].gameObject.SetActive(true);
    }
}
