using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Drag&Drop")] [SerializeField] private GameObject _triggerZombie;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        //_triggerZombie.GetComponent<Monster>().OnDeath.AddListener(VigilanteScenePlay)
    }

    private void VigilanteScenePlay()
    {
        // 자경단 이벤트 씬 실행 메서드
    }

}
