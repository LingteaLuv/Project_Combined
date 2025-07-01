using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class JagyungTimelineControl : MonoBehaviour
{
    [SerializeField] PlayableDirector _pd;
    [SerializeField] PlayerCameraController _pcc;
    [SerializeField] PlayerMovement _pm;

    private WaitForSeconds _wfs;
    private bool _hasRun;
    

    private void Awake()
    { 
        _wfs = new WaitForSeconds((float)_pd.duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasRun && other.CompareTag("Player"))
        {
            _hasRun = true;
            _pd.Play();
            StartCoroutine(DisableControl());
        }
    }

    private IEnumerator DisableControl()
    {
        Debug.Log("진입");
        UIManager.Instance.LockUIUpdate();
        UIManager.Instance.OffHUI();
        UIManager.Instance.OffQuickslot();
        _pm.MoveLock();
        _pcc.PauseCamera();
        yield return _wfs;
        UIManager.Instance.UnlockUIUpdate();
        UIManager.Instance.OffHUI();
        UIManager.Instance.OffQuickslot();
        _pm.MoveLock();
        _pcc.PauseCamera();
    }
}
