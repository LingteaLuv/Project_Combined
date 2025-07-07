using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndingVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip videoClip1;
    public VideoClip videoClip2;
    public VideoClip videoClip3;
    public string nextSceneName = "TitleScene";

    public void PlayEndingVideo(int index)
    {
        switch (index)
        {
            case 1:
                videoPlayer.clip = videoClip3;
                break;
            case 2:
                videoPlayer.clip = videoClip3;
                break;
            case 3:
                videoPlayer.clip = videoClip3;
                break;
            default:
                return;
        }
        videoPlayer.loopPointReached -= OnVideoEnd;
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();
    }

    private void OnVideoEnd(VideoPlayer vidio)
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
        if (videoPlayer.isPlaying && Input.GetKeyDown(KeyCode.Escape))
        {
            videoPlayer.Stop();
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
