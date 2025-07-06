using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMark : MonoBehaviour
{
    [SerializeField] public Image _panel;
    [SerializeField] public Image _n;
    [SerializeField] public Image _m;


    public void SetNone()
    {
        _n.enabled = false;
        _m.enabled = false;
    }

    public void SetAvailable()
    {
        _n.enabled = false;
        _m.enabled = true;
    }

    public void SetCompleted()
    {
        _n.enabled = true;
        _m.enabled = false;
    }
}



