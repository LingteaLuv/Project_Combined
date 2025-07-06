using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WomanTimeline : TimelineControl
{
    [SerializeField] PlayerNPCInteractor _pni;
    [SerializeField] string _startNPCID;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            _pd.Play();
        }
    }
    private void Start()
    {
        _pni.OnInteract3 += StartTL;
    }
    private void StartTL(string id)
    {
        if (!_hasRun && id == _startNPCID)
        {
            _hasRun = true;
            _pd.Play();
            StartCoroutine(DisableControl());
        }
    }

    protected override IEnumerator DisableControl()
    {
        UIManager.Instance.OffHUI();
        UIManager.Instance.OffQuickslot();
        yield return _wfs;
        UIManager.Instance.UnlockUIUpdate();
        UIManager.Instance.OffHUI();

        UIManager.Instance.OffQuickslot();
        UISceneLoader.Instance.Playerattack.IsAttacking = false;
        _pm.MoveLock();
        _pcc.PauseCamera();
    }




}
