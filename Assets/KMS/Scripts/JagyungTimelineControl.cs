using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class JagyungTimelineControl : TimelineControl
{

    [SerializeField] GameObject _wall;

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasRun && other.CompareTag("Player"))
        {
            _wall.SetActive(false);
            _hasRun = true;
            _pd.Play();
            StartCoroutine(DisableControl());
        }
    }
}
