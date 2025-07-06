using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatertankTimelineControl : TimelineControl
{
    [SerializeField] Transform _player;
    [SerializeField] Transform _start;
    [SerializeField] Transform _last;

    [SerializeField] PlayerNPCInteractor _pni;

    [SerializeField] private string _startNPCID;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            _hasRun = true;
            _pd.Play();
            StartCoroutine(Position());
            StartCoroutine(DisableControl());
        }
    }
    private void Start()
    {
        //_pni.OnInteract3 += StartTL;
    }

    public void StartTL()
    {
        if (!_hasRun)
        {
            _hasRun = true;
            _pd.Play();
            StartCoroutine(Position());
            StartCoroutine(DisableControl());
        }
    }

    private IEnumerator Position()
    {
        yield return new WaitForSeconds(0.7f);
        _player.position = _start.position;
        yield return new WaitForSeconds((float)_pd.duration - 1f);
        _player.position = _last.position;
    }
}
