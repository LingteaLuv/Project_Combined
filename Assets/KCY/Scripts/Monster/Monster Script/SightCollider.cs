using TMPro;
using UnityEngine;

public class SightCollider : MonoBehaviour
{
    private Monster_temp _monster;


    private void Awake()
    {
        _monster = GetComponent<Monster_temp>();
    }
    private void OnTriggerEnter(Collider other)
    {
        _monster.SightDetectPlayer(other);
    }

    private void OnTriggerStay(Collider other)
    {
        _monster.SightDetectPlayer(other);
    }
    private void OnTriggerExit(Collider other)
    {
        // 플레이어 아니면 나가고
        if (((1 << other.gameObject.layer) & _monster.PlayerLayerMask) == 0) return;

        if (_monster.IsDetecting && _monster.TargetPosition == other.transform)
        {
            Debug.Log(" 플레이어 감지 해제 → Idle 상태 복귀");
            _monster.TargetPosition = null;
            _monster.IsDetecting = false;

            // 감지해서 나간 대상이 플레이어였을 경우 해당 위치를 임시 포인트로 잡고 해당 방향을 기준으로 패트롤을 진행한다.
            _monster.TempPoint = other.transform;
            _monster._monsterMerchine.ChangeState(_monster._monsterMerchine.StateDic[Estate.Idle]);
        }
    }

}
