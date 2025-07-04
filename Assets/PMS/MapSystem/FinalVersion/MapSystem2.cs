using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapSystem2 : MonoBehaviour
{
    [Header("Map Settings")]
    public GameObject _mapUI;
    public RawImage _mapImage;
    public Button _mapCloseButton;
    private Color _color;
    private bool _isFadingIn = false;
    private bool _isFadingOut = false;
    private float fadeDuration = 1f;
    private float fadeTimer = 0f;

    private bool _isMapOpen = false;
    //[SerializeField] private Stack<GameObject> _visitFog; 추후 Stack을 통한 방문된 안개 FadeIn,FadeOut 처리

    private void Start()
    {
        InitializeMapUI();
    }

    private void InitializeMapUI()
    {
        _mapCloseButton = _mapUI.transform.GetComponentInChildren<Button>();                                                   //비활성화 처리
        _mapImage = _mapUI.transform.GetComponentInChildren<RawImage>();
        if (_mapImage != null)
        {
            _color = _mapImage.color;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && (!_isFadingIn && !_isFadingOut))
        {
            ToggleMap();
        }
        if (_isFadingIn)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, fadeTimer / fadeDuration);
            _mapImage.color = new Color(_color.r, _color.g, _color.b, alpha);

            if (fadeTimer >= fadeDuration)
            {
                _isFadingIn = false;
                _mapCloseButton.enabled = true;
            }
        }
        else if (_isFadingOut)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
            _mapImage.color = new Color(_color.r, _color.g, _color.b, alpha);

            if (fadeTimer >= fadeDuration)
            {
                _isFadingOut = false;
                _mapUI.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 토글형식 지도 껏다켜기
    /// </summary>
    public void ToggleMap()
    {
        _isMapOpen = !_isMapOpen;
        if (_isMapOpen)
        {
            _mapUI.gameObject.SetActive(true);
            MapOpenFadeIn();
        }
        else
        {
            MapCloseFadeOut();
        }
    }

    private void MapOpenFadeIn()
    {
        fadeTimer = 0f;
        _isFadingIn = true;
        _isFadingOut = false;
        _mapCloseButton.enabled = false;
    }
    private void MapCloseFadeOut()
    {
        fadeTimer = 0f;
        _isFadingOut = true;
        _isFadingIn = false;
        _mapCloseButton.enabled = false;
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if(_isMapOpen == false)
        {
            _visitFog.Push(collision.gameObject);
        }
    }*/


    /*private void FogClearing(Stack<GameObject> _visitFogStack)
    {

    }*/
}


