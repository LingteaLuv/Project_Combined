using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 일단 구현 끝나고 
public class NewBehaviourScript : MonoBehaviour
{
    private Monster_temp _monster;

    private void Awake()
    {
        _monster = GetComponent<Monster_temp>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //_monster.SightDetectPlayer(other);
    }

    private void OnTriggerStay(Collider other)
    {
        //_monster.SightDetectPlayer(other);
    }
    private void OnTriggerExit(Collider other)
    {
        
       
    }
}
