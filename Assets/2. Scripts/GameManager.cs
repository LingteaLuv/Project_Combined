using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Todo : 게임 시작 메서드
    }
    
    private void Init()
    {
        // 게임 매니저 자체 초기화 메서드
    }
}
