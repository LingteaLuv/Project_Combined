using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
  
                if (_instance == null && Application.isPlaying)
                {
                    var obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    // 파괴 여부 옵션 플래그 : 기본값은 true로 설정
    protected virtual bool ShouldDontDestroy => true;


    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            
            if(ShouldDontDestroy)
            {
                DontDestroyOnLoad(gameObject);
            }         
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
