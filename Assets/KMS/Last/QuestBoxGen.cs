using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoxGen : MonoBehaviour
{

    [SerializeField] GameObject _civil01;
    [SerializeField] Vector3 _pos1;
    [SerializeField] GameObject _v02;
    [SerializeField] Vector3 _pos2;
    void Start()
    {
        QuestManager.Instance.OnQuestAccepted += GenBox;
    }

    private void GenBox(QuestData q , QuestProgress qp)
    {
        if(q.QuestID == "Civil01")
        {
            Instantiate(_civil01, _pos1, Quaternion.identity);
        }
        else if (q.QuestID == "V02")
        {
            Instantiate(_v02, _pos2, Quaternion.identity);
        }
    }
}
