using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatertankTrigger : MonoBehaviour
{
    [SerializeField] WatertankTimelineControl _wt;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _wt.StartTL();
        }
    }
}
