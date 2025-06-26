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
        HudController hudController = FindObjectOfType<HudController>();
        if (hudController != null)
        {
            hudController.Init(_playerProperty);
        }
        else
        {
            Debug.LogError("HudManager를 찾을 수 없습니다");
        }
    }
}
