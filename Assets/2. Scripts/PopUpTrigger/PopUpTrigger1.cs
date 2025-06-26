using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTrigger1: MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            TextManager.Instance.PopupTextForSecond("1001",3);
        }
        
    }
    
}
