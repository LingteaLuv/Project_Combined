using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMediator : MonoBehaviour
{
    [Header("Drag&Drop")] 
    [SerializeField] private PlayerProperty _playerProperty;

    private void Start()
    {
        Init();
    }
    
    private void Init()
    {
        HudManager hudManager = FindObjectOfType<HudManager>();
        if (hudManager != null)
        {
            hudManager.Init(_playerProperty);
        }
        else
        {
            Debug.LogError("HudManager를 찾을 수 없습니다");
        }
    }
}
