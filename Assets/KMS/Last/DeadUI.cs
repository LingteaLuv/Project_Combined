using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadUI : MonoBehaviour
{
    private PlayerProperty _pp;

    [SerializeField] private GameObject UI;

    private void Start()
    {
        UI.SetActive(false);
        _pp = UISceneLoader.Instance.Playerattack.GetComponent<PlayerProperty>();
        _pp.OnDied += OnDied;
    }

    private void OnDied()
    {
        UI.SetActive(true);
        UIManager.Instance.Lock();
    }


    public void TitleButton()
    {
        UIManager.Instance.UnLock();
        UIManager.Instance.CursorUnlock();
        SceneManager.LoadScene("TitleScene");
    }
}
