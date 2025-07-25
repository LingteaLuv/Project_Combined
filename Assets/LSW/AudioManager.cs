using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : Singleton<AudioManager>
{
    [Header("Drag&Drop")]
    [SerializeField] private GameObject _sfxPrefab;
    [SerializeField] private GameObject _uiSfxPrefab;
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private List<AudioClip> _sfxList;
    //  석규 추가
    [SerializeField] private List<AudioClip> _bgmList;

    private Dictionary<string, AudioClip> _bgmDic;
    public Dictionary<string, AudioClip> _sfxDic;

    private Queue<AudioSource> _sfxPool;
    private Queue<AudioSource> _uiSfxPool;

    private bool _isPlayed;
    private int _amount;
    private float _curBGMVolume;
    private float _curSFXVolume;
    // 석규 추가
    private Queue<AudioClip> _bgmQueue = new Queue<AudioClip>();

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

    public void PlayBGM(AudioClip clipName, bool loop)
    {
        //if (_bgmDic.TryGetValue(clipName, out AudioClip audioClip))
        {
            if (_isPlayed && _bgmSource.clip == clipName) return;
            _bgmSource.spatialBlend = 0;
            _bgmSource.clip = clipName;
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
        if (audioClip != null)
        {
            AudioSource audioSource = GetSfxSource();
            audioSource.transform.position = position;
            audioSource.volume = _curSFXVolume;
            audioSource.spatialBlend = 1f;
            audioSource.PlayOneShot(audioClip);
            StartCoroutine(WaitSFX(audioClip.length, audioSource));
        }
    }

    public void PlaySFX(string clipName, Vector3 position)
    {
        if (_sfxDic.ContainsKey(clipName))
        {
            AudioSource audioSource = GetSfxSource();
            audioSource.transform.position = position;
            audioSource.volume = _curSFXVolume;
            audioSource.spatialBlend = 1f;
            audioSource.PlayOneShot(_sfxDic[clipName]);
            StartCoroutine(WaitSFX(_sfxDic[clipName].length, audioSource));
        }
    }

    public void PlayUISFX(AudioClip audioClip)
    {
        if (audioClip != null)
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

        _sfxDic = new Dictionary<string, AudioClip>();
        for (int i = 0; i < _sfxList.Count; i++)
        {
            _sfxDic.Add(_sfxList[i].name, _sfxList[i]);
        }
        EnqueueBGM();
    }

    // 석규 추가
    public void EnqueueBGM()
    {
        foreach (var bgmName in _bgmList)
        {
            _bgmQueue.Enqueue(bgmName);
        }
    }

    // 다음 BGM 재생
    public void PlayNextBGMInQueue()
    {
        if (_bgmQueue.Count == 0) return;
        AudioClip nextClip = _bgmQueue.Dequeue();
        PlayBGM(nextClip, false);

        //  꺼낸 곡을 다시 큐 뒤에 추가
        _bgmQueue.Enqueue(nextClip);
    }

    // 자동 재생 
    public void PlayBgms(bool bgmPlay)
    {
        if (_bgmSource == null) return;

        if (_bgmSource.isPlaying == false && _bgmQueue.Count > 0)
        {
            PlayNextBGMInQueue();
        }
    }
}
