using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class PlayerLooting : MonoBehaviour
{
    [SerializeField] LayerMask _interactLayer;

    private List<Collider> _colliders = new List<Collider>();

    private Collider _lootableColl = null;

    private Lootable _lootable = null;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) TryLoot();
    }
    private void OnTriggerEnter(Collider other) //레이어 6 콜라이더랑 만났을 경우 리스트에 추가
    {
        if (other.gameObject.layer == 6)
        {
            _colliders.Add(other);
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_colliders.Count == 0) return;
        float distance = float.MaxValue;
        Collider near = null;
        foreach ( Collider c in _colliders)
        {
            float temp = (transform.position - c.transform.position).magnitude;
            if (temp < distance)
            {
                distance = temp;
                near = c;
            } // 리스트 내 콜라이더 중 가장 가까운 것 구해서 near에 저장
        }
        if (_lootableColl == near)
        {
            return;
        }
        else
        {
            _lootable = near.GetComponent<Lootable>();
            _lootable.OnOutline();
            _lootable.FUIController.OffDark();
            if (_lootableColl != null)
            {
                Lootable temp = _lootableColl.GetComponent<Lootable>();
                temp.OffOutline();
                temp.FUIController.OnDark();
            }
            _lootableColl = near;
            LootManager.Instance.NewLootableChecked(_lootable);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            _colliders.Remove(other);
        }
        if (_lootableColl == other)
        {
            _lootableColl = null;
            _lootable.OffOutline();
            _lootable.FUIController.OnDark();
            _lootable = null;
            LootManager.Instance.LootableNotExist();
        }
    }

    public void TryLoot()
    {
        if (_lootable == null) return;
        LootManager.Instance.OpenLootTable();

    }
}
