using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class JagyungTimelineControl : TimelineControl
{

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasRun && other.CompareTag("Player"))
        {
            _hasRun = true;
            _pd.Play();
            StartCoroutine(DisableControl());
        }
    }
}
