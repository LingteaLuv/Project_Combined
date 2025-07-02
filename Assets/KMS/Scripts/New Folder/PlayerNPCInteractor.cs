using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNPCInteractor : MonoBehaviour
{

    private List<Collider> _colliders = new List<Collider>();
    private Dictionary<Collider, NPCInteractable> _interactables = new Dictionary<Collider, NPCInteractable>();

    private Collider _lootableColl = null;

    private NPCInteractable _interactable = null;
    private void OnTriggerEnter(Collider other) //레이어 6 콜라이더랑 만났을 경우 리스트에 추가
    {
        if (other.gameObject.layer == 8)
        {
            _colliders.Add(other);
            _interactables.Add(other, other.GetComponentInChildren<NPCInteractable>());

        }
    }

    private void OnTriggerStay(Collider other) // 들어온 것들 중 루팅 가능한 것, 가장 가까운 것 가리기
    {
        if (_colliders.Count == 0) return;
        if (UIManager.Instance.IsUIOpened.Value) return; //UI상태에선 아래 작업 하지 않음
        float distance = float.MaxValue;
        Collider near = null;
        foreach (Collider c in _colliders)
        {
            if (c == null)
            {
                continue;
            }
            float temp = (transform.position - c.transform.position).magnitude;
            if (temp < distance)
            {
                distance = temp;
                near = c;
            } // 리스트 내 루팅 가능한 콜라이더 중 가장 가까운 것 구해서 near에 저장
        }
        if (near == null) //루팅 가능한 콜라이더가 없었다. 
        {
            return;
        }
        if (_lootableColl == near) // 가장 가까운것과 이미 선택된 것이 같음
        {
            return;
        }
        else //다른 것이 선택되어 있었음 (또는 선택된 것이 없었음) -> 변경
        {
            _interactable = _interactables[near]; 
            _interactable.OnBallon();
            if (_lootableColl != null) 
            {
                NPCInteractable temp = _interactables[_lootableColl];
                temp.OffBallon();
            }
            _lootableColl = near;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            _colliders.Remove(other);
            _interactables.Remove(other);
        }
        if (_lootableColl == other) //선택되었던 것이 나가는 상황
        {
            _lootableColl = null;
            _interactable.OffBallon();
            _interactable = null;
        }
    }

    public bool TryInteract()
    {
        if (_interactable == null) return false;
        DialogueManager.Instance.SetDialogue(_interactable.Dialogue);
        return true;

    }
}
