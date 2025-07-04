using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineControl : MonoBehaviour
{
    [SerializeField] protected PlayableDirector _pd;
    [SerializeField] protected PlayerCameraController _pcc;
    [SerializeField] protected PlayerMovement _pm;

    protected WaitForSeconds _wfs;
    protected bool _hasRun;


    protected void Awake()
    {
        _wfs = new WaitForSeconds((float)_pd.duration);
        _hasRun = false;
    }

    protected virtual IEnumerator DisableControl()
    {
        UIManager.Instance.LockUIUpdate();
        UIManager.Instance.OffHUI();
        UIManager.Instance.OffQuickslot();
        UISceneLoader.Instance.Playerattack.IsAttacking = true;
        _pm.MoveLock();
        _pcc.PauseCamera();
        yield return _wfs;
        UIManager.Instance.UnlockUIUpdate();
        UIManager.Instance.OffHUI();

        UIManager.Instance.OffQuickslot();
        UISceneLoader.Instance.Playerattack.IsAttacking = false;
        _pm.MoveLock();
        _pcc.PauseCamera();
    }
}
