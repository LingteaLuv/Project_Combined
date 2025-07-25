
using System.Collections.Generic;

using UnityEngine;

public class LootDetector : MonoBehaviour
{
    [SerializeField] LayerMask _interactLayer;

    private List<Lootable> _lootables = new List<Lootable>();
    private void OnTriggerEnter(Collider other) //레이어 6 콜라이더랑 만났을 경우 리스트에 추가
    {
        if (other.gameObject.layer == 6 || other.gameObject.layer == 13)
        {
            _lootables.Add(other.GetComponentInChildren<Lootable>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_lootables.Count == 0) return;
        foreach ( Lootable c in _lootables)
        {
            if (c == null) continue;
            if (c.IsLootable)
            {
                c.FUIController.OnFUI();
            }
            else
            {
                c.FUIController.OffFUI();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6 || other.gameObject.layer == 13)
        {
            Lootable temp = other.GetComponentInChildren<Lootable>();
            temp.FUIController.OffFUI();
            _lootables.Remove(temp);
        }
    }
}
