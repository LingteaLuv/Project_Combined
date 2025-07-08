using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndingVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer1;
    public VideoPlayer videoPlayer2;
    public VideoPlayer videoPlayer3;
    public string nextSceneName = "TitleScene";

    private VideoPlayer currentPlayer;

    /// <summary>
    /// index에 따라 VideoPlayer 하나만 재생, 나머지는 꺼짐
    /// </summary>
    public void PlayEndingVideo(int index)
    {
        // 모두 끄기
        if (videoPlayer1 != null)
        {
            videoPlayer1.gameObject.SetActive(false);
            videoPlayer1.loopPointReached -= OnVideoEnd;
        }
        if (videoPlayer2 != null)
        {
            videoPlayer2.gameObject.SetActive(false);
            videoPlayer2.loopPointReached -= OnVideoEnd;
        }
        if (videoPlayer3 != null)
        {
            videoPlayer3.gameObject.SetActive(false);
            videoPlayer3.loopPointReached -= OnVideoEnd;
        }

        switch (index)
        {
            case 1:
                currentPlayer = videoPlayer1;
                break;
            case 2:
                currentPlayer = videoPlayer2;
                break;
            case 3:
                currentPlayer = videoPlayer3;
                break;
            default:
                Debug.LogWarning("잘못된 엔딩 번호!");
                return;
        }

        if (currentPlayer != null)
        {
            currentPlayer.gameObject.SetActive(true);
            currentPlayer.loopPointReached += OnVideoEnd;
            currentPlayer.Play();
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }

    private void Start()
    {
        int endingIndex = PlayerPrefs.GetInt("EndingIndex", 3);
        PlayEndingVideo(endingIndex);
    }

    private void Update()
    {
        if (currentPlayer != null && currentPlayer.isPlaying && Input.GetKeyDown(KeyCode.Escape))
        {
            currentPlayer.Stop();
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
