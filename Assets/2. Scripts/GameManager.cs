using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public event Action OnGameOver;
    public event Action OnGameClear;

    public GameObject Player;
    
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    public void GameClear()
    {
        OnGameClear?.Invoke();
    }

    
    public void GameOver()
    {
        // Todo : 게임 오버 메서드
        OnGameOver?.Invoke();
    }

    public void GameStart()
    {
        SceneManager.LoadScene("1. Scenes/GameScene",0);
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
        }
        
    }
    
    private void Init()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
        }// 게임 매니저 자체 초기화 메서드
    }
}
