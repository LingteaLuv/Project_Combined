using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTrigger1: MonoBehaviour
{
    private bool _isTriggered = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player")&&!_isTriggered)
        {
            TextManager.Instance.PopupTextForSecond("1001",2);
            _isTriggered = true;
        }
    }
}
