using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class CamaraShaker : MonoBehaviour
{

    private CinemachineVirtualCamera _cmv;
    private CinemachineBasicMultiChannelPerlin _cmp;

    [SerializeField] private NoiseSettings _default;
    [SerializeField] private NoiseSettings _damage;
    [SerializeField] private NoiseSettings _shoot;

    private Coroutine _shakeco;
    private Coroutine _vigco;
    private WaitForSeconds _damagewfs;
    private WaitForSeconds _shootwfs;
    private WaitForEndOfFrame _wff;

    [SerializeField] Volume _vol;
    private Vignette _vig;

    private void Awake()
    {
        _cmv = GetComponentInParent<CinemachineVirtualCamera>();
        _cmp = _cmv.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _damagewfs = new WaitForSeconds(0.2f);
        _shootwfs = new WaitForSeconds(0.2f);
        _vol.profile.TryGet<Vignette>(out _vig);
    }
    private void Start()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        _cmp.m_NoiseProfile = _default;
        _cmp.m_AmplitudeGain = 0.7f;
        _cmp.m_FrequencyGain = 1;
    }
    public void GunShootShake()
    {
        if (_shakeco != null)
        {
            StopCoroutine(_shakeco);
        }
        _shakeco = StartCoroutine(ShakeCo(_shoot, 6, 5));
    }
    public void DamageShake()
    {
        if (_shakeco != null)
        {
            StopCoroutine(_shakeco);
        }
        _shakeco = StartCoroutine(ShakeCo(_damage, 5, 10));
        if (_vigco != null)
        {
            StopCoroutine(_vigco);
        }
        _vigco = StartCoroutine(VigCo());
    }

    private IEnumerator ShakeCo(NoiseSettings type, float amp, float freq)
    {
        _cmp.m_NoiseProfile = type;
        _cmp.m_AmplitudeGain = amp;
        _cmp.m_FrequencyGain = freq;
        if (type == _shoot)
        {
            yield return _shootwfs;
        }
        else if (type == _damage)
        {
            yield return _damagewfs;
        }
        SetDefault();
    }

    private IEnumerator VigCo()
    {
        _vig.intensity.value = 0.5f;
        while (_vig.intensity.value > 0)
        {
            _vig.intensity.value -= 0.02f;
            yield return _wff;
            yield return _wff;
        }
    }
}
