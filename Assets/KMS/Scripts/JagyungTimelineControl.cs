using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class JagyungTimelineControl : MonoBehaviour
{
    [SerializeField] PlayableDirector _pd;

    private bool _already;

    private void OnTriggerEnter(Collider other)
    {
        if (!_already && other.tag == "Player")
        {
            _already = true;
            _pd.Play();
        }
    }
}
