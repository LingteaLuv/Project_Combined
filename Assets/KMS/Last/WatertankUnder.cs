using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatertankUnder : MonoBehaviour
{
    [SerializeField] GameObject _wall;

    private void OnTriggerEnter(Collider other)
    {
        if (EventManager.Instance.HasLootedOnTank && other.tag == "Player")
        {
            _wall.SetActive(false);
        }
    }
}
