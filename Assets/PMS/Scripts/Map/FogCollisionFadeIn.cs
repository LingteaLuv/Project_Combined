using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogCollisionFadeIn : MonoBehaviour
{
    private Renderer _render;
    private Color _color;

    private bool _iscollision = false;
    private bool _isFading = false;
    public bool IsFading { get { return _isFading; } set { _isFading = value; } }

    
    [SerializeField]private float _fadeDuration = 5f;
    private float _fadeTimer = 0f;

    void Start()
    {
        _render = GetComponent<Renderer>();
        if (_render != null)
        {
            //개별적인 머터리얼 컬러에 접근
            _color = _render.material.color;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 충돌 시 페이드 아웃 시작
        if (!_iscollision)
        {
            _isFading = true;
            _iscollision = true;
            _fadeTimer = 0f;
        }
    }

    void Update()
    {
        if (_isFading && _render != null)
        {
            _fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(_color.a, 0f, _fadeTimer / _fadeDuration);
            Color newColor = new Color(_color.r, _color.g, _color.b, alpha);
            _render.material.color = newColor;

            if (_fadeTimer >= _fadeDuration)
            {
                Destroy(gameObject);
            }
        }
    }
}
