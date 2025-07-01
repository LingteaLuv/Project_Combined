using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public void GameOver()
    {
        // Todo : 게임 오버 메서드
    }

    public void GameStart()
    {
        SceneManager.LoadScene("1. Scenes/GameScene",0);
    }
    
    private void Init()
    {
        // 게임 매니저 자체 초기화 메서드
    }
}
