using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [Header("Drag&Drop")]
    [SerializeField] private Button _startBtn;
    //[SerializeField] private Button _saveBtn;
    //[SerializeField] private Button _loadBtn;
    [SerializeField] private Button _settingBtn;
    [SerializeField] private Button _exitBtn;
    [SerializeField] private Button _infoBtn;

    //[SerializeField] private GameObject _saveUI;
    //[SerializeField] private GameObject _loadUI;
    [SerializeField] private GameObject _settingUI;
    [SerializeField] private GameObject _infoUI;

    private void Start()
    {
        _startBtn.onClick.AddListener(() => GameManager.Instance.GameStart());
        //_saveBtn.onClick.AddListener(() => _saveUI.SetActive(true));
        //_loadBtn.onClick.AddListener(() => _loadUI.SetActive(true));
        _settingBtn.onClick.AddListener(() => _settingUI.SetActive(true));
        _exitBtn.onClick.AddListener(() =>
        {
            Debug.Log("게임 종료");
            Application.Quit();
        });
        _infoBtn.onClick.AddListener(() => _infoUI.SetActive(true));
    }
}
