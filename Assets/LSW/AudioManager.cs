using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class AudioManager : Singleton<AudioManager>
{
    [Header("Drag&Drop")] 
    [SerializeField] private GameObject _sfxPrefab;
    [SerializeField] private GameObject _uiSfxPrefab;
    [SerializeField] private AudioSource _bgmSource;

    private Dictionary<string, AudioClip> _bgmDic;
    
    private Queue<AudioSource> _sfxPool;
    private Queue<AudioSource> _uiSfxPool;
    
    private bool _isPlayed;
    private int _amount;
    private float _curBGMVolume;
    private float _curSFXVolume;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    
    private void Start()
    {
        _curBGMVolume = SettingManager.Instance.BGMSound.Value;
        _curSFXVolume = SettingManager.Instance.SFXSound.Value;
        SettingManager.Instance.BGMSound.OnChanged += BGMSoundUpdate;
        SettingManager.Instance.SFXSound.OnChanged += SFXSoundUpdate;
    }
    
    private void BGMSoundUpdate(float value)
    {
        _curBGMVolume = value;
        _bgmSource.volume = _curBGMVolume;
    }
    
    private void SFXSoundUpdate(float value)
    {
        _curSFXVolume = value;
    }
    
    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            SettingManager.Instance.BGMSound.OnChanged -= BGMSoundUpdate;
            SettingManager.Instance.SFXSound.OnChanged -= SFXSoundUpdate;
        }
    }
    
    public void PlayBGM(string clipName, bool loop)
    {
        if(_bgmDic.TryGetValue(clipName, out AudioClip audioClip))
        {
            if (_isPlayed && _bgmSource.clip == audioClip) return;
            _bgmSource.spatialBlend = 0;
            _bgmSource.clip = audioClip;
            _bgmSource.loop = loop;
            _bgmSource.Play();
            StartCoroutine(BGMFadeIn(_bgmSource, 3f));
            _isPlayed = true;
        }
    }

    public void StopBGM()
    {
        StartCoroutine(BGMFadeOut(_bgmSource, 1f));
        _isPlayed = false;
    }

    public void PlaySFX(AudioClip audioClip, Vector3 position)
    {
        if(audioClip != null)
        {
            AudioSource audioSource = GetSfxSource();
            audioSource.transform.position = position;
            audioSource.volume = _curSFXVolume;
            audioSource.spatialBlend = 1f;
            audioSource.PlayOneShot(audioClip);
            StartCoroutine(WaitSFX(audioClip.length, audioSource));
        }
    }
    
    public void PlayUISFX(AudioClip audioClip)
    {
        if(audioClip != null)
        {
            AudioSource audioSource = GetUISfxSource();
            audioSource.volume = _curSFXVolume;
            audioSource.spatialBlend = 0f;
            audioSource.PlayOneShot(audioClip);
            StartCoroutine(WaitSFX(audioClip.length, audioSource, true));
        }
    }
    
    private AudioSource GetUISfxSource()
    {
        if (_uiSfxPool.Count > 0)
        {
            return _uiSfxPool.Dequeue();
        }

        GameObject temp = Instantiate(_uiSfxPrefab);
        return temp.GetComponent<AudioSource>();
    }
    
    private AudioSource GetSfxSource()
    {
        if (_sfxPool.Count > 0)
        {
            return _sfxPool.Dequeue();
        }

        GameObject temp = Instantiate(_sfxPrefab);
        return temp.GetComponent<AudioSource>();
    }
    
    private IEnumerator WaitSFX(float delay, AudioSource source, bool isUI = false)
    {
        yield return new WaitForSeconds(delay);
        source.Stop();
        source.clip = null;
        if (isUI)
        {
            _uiSfxPool.Enqueue(source);
        }
        else
        {
            _sfxPool.Enqueue(source);
        }
    }
    
    private IEnumerator BGMFadeIn(AudioSource source, float fadeTime)
    {
        float timer = 0f;
        while (fadeTime > timer)
        {
            source.volume = Mathf.Lerp(0f, _curBGMVolume, timer / fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        source.volume = _curBGMVolume;
    }

    private IEnumerator BGMFadeOut(AudioSource source, float fadeTime)
    {
        float timer = 0f;
        while (fadeTime > timer)
        {
            source.volume = Mathf.Lerp(_curBGMVolume, 0f, timer / fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }
        source.Stop();
        source.volume = _curBGMVolume;
    }
    
    private void Init()
    {
        _amount = 10;
        _sfxPool = new Queue<AudioSource>();
        _uiSfxPool = new Queue<AudioSource>();
        for (int i = 0; i < _amount; i++)
        {
            AudioSource sfx = Instantiate(_sfxPrefab).GetComponent<AudioSource>();
            AudioSource uiSfx = Instantiate(_uiSfxPrefab).GetComponent<AudioSource>();
            _sfxPool.Enqueue(sfx);
            _uiSfxPool.Enqueue(uiSfx);
        }
    }
}

