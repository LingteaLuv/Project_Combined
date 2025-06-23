using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 컴포넌트 클래스를 싱글톤으로 만드는 추상 클래스입니다.
// 컴포넌트 클래스 선언은
// public class CompoClass : Singleton<CompoClass> { }
public abstract class SingletonT<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected void SetInstance()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
