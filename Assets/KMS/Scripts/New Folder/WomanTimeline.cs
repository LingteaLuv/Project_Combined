using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WomanTimeline : TimelineControl
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            _pd.Play();
        }
    }
}
