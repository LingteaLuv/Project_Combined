using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class NightVolumeControll : MonoBehaviour
{
    [SerializeField] Volume _vol;
    private Vignette _vig;
    private WaitForEndOfFrame _wfef;
    private WaitForSeconds _wfs;

    public float FogDens;
    public float VigIntens;
    public float FogAmount;
    public float VigAmount;

    private void Awake()
    {
        _wfef = new WaitForEndOfFrame();
        _wfs = new WaitForSeconds(0.5f);
        _vol.profile.TryGet<Vignette>(out _vig);
    }
    void Start()
    {
        // 안개 활성화
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0f;
        _vig.intensity.Override(0f);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(NightFog());
            StartCoroutine(NightVig());
        }
    }

    private IEnumerator NightFog()
    {
        while (true)
        {
            RenderSettings.fogDensity += FogAmount;
            yield return _wfs;
            if (RenderSettings.fogDensity >= FogDens)
            {
                RenderSettings.fogDensity = FogDens;
                break;
            }
        }
    }

    private IEnumerator NightVig()
    {
        while (true)
        {
            _vig.intensity.value += VigAmount;
            yield return _wfs;
            if (_vig.intensity.value >= VigIntens)
            {
                _vig.intensity.value = VigIntens;
                break;
            }
        }
    }

}
