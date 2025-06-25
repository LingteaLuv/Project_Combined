using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneLoader : MonoBehaviour
{
    [SerializeField] public ItemHolder Right;
    [SerializeField] public ItemHolder Left;
    private void Start()
    {
        //SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
        StartCoroutine(dStart());
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

        Right.Subscribe();
        Left.Subscribe();
    }
}
