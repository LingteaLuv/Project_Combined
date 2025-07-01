using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class JagyungTimelineControl : MonoBehaviour
{
    [SerializeField] PlayableDirector _pd;
    [SerializeField] PlayerCameraController _pcc;

    private WaitForSeconds _wfs;
    private bool _hasRun;
    

    private void Awake()
    { 
        _wfs = new WaitForSeconds((float)_pd.duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasRun && other.tag == "Player")
        {
            _hasRun = true;
            _pd.Play();
            StartCoroutine(DisableControl());
        }
    }

    private IEnumerator DisableControl()
    {
        UIManager.Instance.LockUIUpdate();
        UIManager.Instance.OffHUI();
        UIManager.Instance.OffQuickslot();
        yield return _wfs;
        UIManager.Instance.UnlockUIUpdate();
        UIManager.Instance.OffHUI();
        UIManager.Instance.OffQuickslot();
    }
}
