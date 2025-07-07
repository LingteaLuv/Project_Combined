using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUIControl : MonoBehaviour
{
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject SettingUI;
    [SerializeField] public CanvasGroup UIGroup;

    public Property<bool> IsPaused;

    private PlayerCameraController _pcc;
    private WaitForSecondsRealtime _wait;

    public bool PauseLock;

    private Coroutine _co;

    private void Awake()
    {
        _wait = new WaitForSecondsRealtime(0.02f);
        _pcc = UISceneLoader.Instance.Playerattack.GetComponent<PlayerCameraController>();
        IsPaused = new Property<bool>(false);
        PauseUI.SetActive(false);
        UIGroup.alpha = 0;
    }
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.Instance.IsUIOpened.Value) return;
        if (PauseLock) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    public void LockPause()
    {
        PauseLock = true;
    }
    public void UnlockPasue()
    {
        PauseLock = false;
    }

    public void Title()
    {
        IsPaused.Value = false;
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
        SceneManager.UnloadSceneAsync("UIScene");
        SceneManager.UnloadSceneAsync("GameScene");
        SceneManager.UnloadSceneAsync("Demo_City_Universal_RenderPipeline");
    }

    private void Open()
    {
        IsPaused.Value = true;
        PauseUI.SetActive(true);
        SettingUI.SetActive(true);
        _pcc.PauseCamera();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager.Instance.LockUIUpdate();
        if (_co != null)
        {
            StopCoroutine(_co);
        }
        _co = StartCoroutine(FadeIn());
        Time.timeScale = 0f;

    }
    private void Close()
    {
        if (_co != null)
        {
            StopCoroutine(_co);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _co = StartCoroutine(FadeOut());
    }
    public void Toggle()
    {
        if (IsPaused.Value)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private IEnumerator FadeIn()
    {
        while (UIGroup.alpha < 1)
        {
            UIGroup.alpha += 0.03f;
            yield return _wait;
        }
        UIGroup.alpha = 1f;
    }
    private IEnumerator FadeOut()
    {
        while (UIGroup.alpha > 0)
        {
            UIGroup.alpha -= 0.03f;
            yield return _wait;
        }
        UIGroup.alpha = 0f;
        IsPaused.Value = false;
        SettingUI.SetActive(false);
        PauseUI.SetActive(false);
        UIManager.Instance.UnlockUIUpdate();
        _pcc.PauseCamera();
        Time.timeScale = 1f;
    }
}
