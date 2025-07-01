using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyInfoUI : MonoBehaviour
{
    [Header("Drag&Drop")]
    [SerializeField] private Button _closeBtn;

    private void Start()
    {
        gameObject.SetActive(false);
        Init();
    }

    private void Init()
    {
        _closeBtn.onClick.AddListener(() => gameObject.SetActive(false));
    }
}
