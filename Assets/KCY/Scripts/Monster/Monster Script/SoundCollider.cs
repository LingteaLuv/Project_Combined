using UnityEngine;

public class SoundCollider : MonoBehaviour
{
    private Monster_temp _monster;

    private void Awake()
    {
        _monster = GetComponentInParent<Monster_temp>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _monster.PlayerLayerMask) != 0 ||
            ((1 << other.gameObject.layer) & _monster.SoundLayerMask) != 0)
        {
            _monster.SoundDetectPlayer(other);
            _monster.IsEventActive = true; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & _monster.PlayerLayerMask) != 0 ||
            ((1 << other.gameObject.layer) & _monster.SoundLayerMask) != 0)
        {
            _monster.IsEventActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & _monster.PlayerLayerMask) == 0 &&
            ((1 << other.gameObject.layer) & _monster.SoundLayerMask) == 0)
            return;

        // 감지했던 타겟이 사라졌다면 감지 종료
        if (_monster.TargetPosition == other.transform || _monster.IsEventActive)
        {
            _monster.IsEventActive = false;
            Debug.Log("사운드 감지 종료 IsEventActive = false");

            // TempPoint 감지하던 위치로 고정
            _monster.TempPoint = other.transform.position;

            // 상태 리셋 (Patrol은 GoToEvent에서 자체적으로 전환됨)
        }
    }
}