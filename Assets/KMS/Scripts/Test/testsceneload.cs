using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testsceneload : SingletonT<testsceneload>
{
    private void Awake()
    {
        SetInstance();
        SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
    }
}
