using System.Collections;
using System.Collections.Generic;
 
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneLoader : SingletonT<UISceneLoader>
{
    [SerializeField] public PlayerAttack Playerattack;
    private void Awake()
    {
        SetInstance();
        StartCoroutine(dStart());
    }
    private void Start()
    {
        //SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
        
    }
    IEnumerator dStart()
    {
        // Additive 씬 비동기 로드 시작
        AsyncOperation loadOp = SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);

        // 씬 로딩이 완료될 때까지 대기
        while (!loadOp.isDone)
        {
            yield return null;
        }

        //Right.Subscribe();
        //Left.Subscribe();
    }
}
