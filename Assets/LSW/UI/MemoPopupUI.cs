using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MemoPopupUI : MonoBehaviour
{
    [Header("Drag&Drop")] 
    [SerializeField] private TMP_Text _popUpText;
    [SerializeField] private TMP_Text _popUpHeadText;

    private void Awake()
    {
        Init();
    }

    public void PopUpText(string text)
    {
        _popUpText.text = text;
    }

    public void PupUpHeadText(string text)
    {
        _popUpHeadText.text = text;
    }
    
    public void ResetText()
    {
        _popUpText.text = "";
        _popUpHeadText.text = "";
    }
    
    private void Init()
    {
        if (_popUpText == null)
        {
            _popUpText = GetComponentInChildren<TMP_Text>();
        }
    }
}
