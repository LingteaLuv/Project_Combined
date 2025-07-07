using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "TitleScene"; // 다음 씬 이름

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
