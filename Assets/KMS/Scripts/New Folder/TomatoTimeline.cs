using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoTimeline : TimelineControl
{
    [SerializeField] string _startNPCID;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            _pd.Play();
        }
    }
    private void Start()
    {
        //_pni.OnInteract3 += StartTL;
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
}
