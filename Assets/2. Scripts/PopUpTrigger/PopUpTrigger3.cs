using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTrigger3: MonoBehaviour
{
    private bool _isTriggered = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player")&&!_isTriggered)
        {
            TextManager.Instance.PopupTextForSecond("1003",2);
            _isTriggered = true;
        }
    }
}
