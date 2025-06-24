using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class LootDetector : MonoBehaviour
{
    [SerializeField] LayerMask _interactLayer;

    private List<Lootable> _lootables = new List<Lootable>();
    private void OnTriggerEnter(Collider other) //레이어 6 콜라이더랑 만났을 경우 리스트에 추가
    {
        if (other.gameObject.layer == 6)
        {
            _lootables.Add(other.GetComponentInChildren<Lootable>());
            Debug.Log(1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_lootables.Count == 0) return;
        foreach ( Lootable c in _lootables)
        {
            if (c.IsLootable)
            {
                c.FUIController.OnFUI();
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Lootable temp = other.GetComponentInChildren<Lootable>();
            temp.FUIController.OffFUI();
            _lootables.Remove(temp);
        }
    }
}
