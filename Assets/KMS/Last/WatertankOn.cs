using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatertankOn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EventManager.Instance.IsOnTank = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            EventManager.Instance.IsOnTank = false;
        }
    }
}
