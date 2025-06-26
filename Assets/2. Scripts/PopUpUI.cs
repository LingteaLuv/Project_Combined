using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpUI : MonoBehaviour
{
    [Header("Drag&Drop")] 
    [SerializeField] private TMP_Text _popUpText;
    [SerializeField] private TMP_Text _popUpHeadText;

    private void Awake()
    {
        Init();
    }

    public void PopupText(string text)
    {
        _popUpText.text = text;
    }

    public void PupupHeadText(string text)
    {
        _popUpHeadText.text = text;
    }
    
    public void ResetText()
    {
        _popUpText.text = "";
    }
    
    private void Init()
    {
        if (_popUpText == null)
        {
            _popUpText = GetComponentInChildren<TMP_Text>();
        }
    }
}
