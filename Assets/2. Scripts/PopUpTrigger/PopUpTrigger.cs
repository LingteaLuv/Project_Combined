using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTrigger: MonoBehaviour
{
    [SerializeField] private string _popUpTextId;    
    [SerializeField] private int _popUpTime;    
    
    private bool _isTriggered = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player")&&!_isTriggered)
        {
            TextManager.Instance.PopupTextForSecond(_popUpTextId,_popUpTime);
            _isTriggered = true;
        }
    }
}
