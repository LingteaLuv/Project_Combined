using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 일단 구현 끝나고 
public class SoundCollider : MonoBehaviour
{
    private Monster_temp _monster;

    private void Awake()
    {
        _monster = GetComponentInParent<Monster_temp>();
    }
    private void OnTriggerEnter(Collider other)
    {
        _monster.SoundDetectPlayer(other);
    }

    private void OnTriggerStay(Collider other)
    {
        _monster.SoundDetectPlayer(other);
    }
    private void OnTriggerExit(Collider other)
    {
        // 플레이 또는 사운드 아니면 나가고
        if (((1 << other.gameObject.layer) & _monster.PlayerLayerMask) == 0 && ((1 << other.gameObject.layer) & _monster.SoundLayerMask) == 0) return;

        if (_monster.TargetPosition == other.transform)
        {
            // 감지해서 나간 대상이 플레이어였을 경우 해당 위치를 임시 포인트로 잡고 해당 방향을 기준으로 패트롤을 진행한다.
            _monster.TempPoint = other.transform;
            _monster._monsterMerchine.ChangeState(_monster._monsterMerchine.StateDic[Estate.Idle]);
        }

    }
}
