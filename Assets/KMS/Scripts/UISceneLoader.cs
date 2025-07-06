using System.Collections;
using System.Collections.Generic;
 
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneLoader : Singleton<UISceneLoader>
{
    [SerializeField] public PlayerAttack Playerattack;

    public Transform Player => Playerattack.gameObject.transform;

    protected override bool ShouldDontDestroy => false;
    protected override void Awake()
    {
        base.Awake();
        SceneManager.LoadScene("LSW_map", LoadSceneMode.Additive);
        
        SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);
        //StartCoroutine(DStart());
    }
    IEnumerator DStart()
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
