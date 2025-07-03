using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem2 : MonoBehaviour
{
    [Header("Map Settings")]
    public GameObject _mapUI;

    private bool _isMapOpen = false;

    private void Update()
    {
        // M키로 지도 토글
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    /// <summary>
    /// 토글형식 지도 껏다켜기
    /// </summary>
    public void ToggleMap()
    {
        _isMapOpen = !_isMapOpen;
        _mapUI.SetActive(_isMapOpen);
    }
}


