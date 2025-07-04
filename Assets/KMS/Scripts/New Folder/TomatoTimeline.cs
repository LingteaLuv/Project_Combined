using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoTimeline : TimelineControl
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            _pd.Play();
        }
    }
}
