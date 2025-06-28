 
using System.Collections.Generic;
 
using UnityEngine;

public class PlayerLooting : MonoBehaviour
{
    [SerializeField] LayerMask _interactLayer;

    private List<Collider> _colliders = new List<Collider>();
    //private List<Lootable> _lootables = new List<Lootable>();
    private Dictionary<Collider, Lootable> _lootables = new Dictionary<Collider, Lootable>();

    private Collider _lootableColl = null;

    private Lootable _lootable = null;
    private void OnTriggerEnter(Collider other) //레이어 6 콜라이더랑 만났을 경우 리스트에 추가
    {
        if (other.gameObject.layer == 6)
        {
            _colliders.Add(other);
            _lootables.Add(other, other.GetComponentInChildren<Lootable>());
            
        }
    }

    private void OnTriggerStay(Collider other) // 들어온 것들 중 루팅 가능한 것, 가장 가까운 것 가리기
    {
        if (_colliders.Count == 0) return;
        if (UIManage.Instance.Current == ModalUI.lootTable) return; //UI상태에선 아래 작업 하지 않음
        float distance = float.MaxValue;
        Collider near = null;
        foreach ( Collider c in _colliders)
        {
            if (c == null)
            {
                continue;
            }
            if (!_lootables[c].IsLootable) continue; //루팅 가능한 상태가 아니면 넘김
            float temp = (transform.position - c.transform.position).magnitude;
            if (temp < distance)
            {
                distance = temp;
                near = c;
            } // 리스트 내 루팅 가능한 콜라이더 중 가장 가까운 것 구해서 near에 저장
        }
        if (near == null) //루팅 가능한 콜라이더가 없었다. 
        {
            if (_lootableColl != null) //기존 루트가능한거 딱 하나 있다가 이게 루트 끝나고 불가능이 된 상황
            {
                Lootable temp = _lootables[_lootableColl];
                temp.OffOutline();
                temp.FUIController.OnDark();
            }
            _lootableColl = null;
            _lootable = null;
            return;
        }
        if (_lootableColl == near) // 가장 가까운것과 이미 선택된 것이 같음
        {
            return;
        }
        else //다른 것이 선택되어 있었음 (또는 선택된 것이 없었음) -> 변경
        {
            _lootable = _lootables[near]; //가장 가까운걸로 lootable변경
            _lootable.OnOutline();
            _lootable.FUIController.OffDark();
            if (_lootableColl != null) // 이미 선택된 것이 있었다면 -> 끈다
            {
                Lootable temp = _lootables[_lootableColl];
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
            _lootables.Remove(other);
        }
        if (_lootableColl == other) //선택되었던 것이 나가는 상황
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
        if (_lootable.Type == LootableType.box)
        {
            LootManager.Instance.ToggleUI();
        }
        else if (_lootable.Type == LootableType.pickup)
        {
            LootManager.Instance.Pickup();
        }

    }
}
