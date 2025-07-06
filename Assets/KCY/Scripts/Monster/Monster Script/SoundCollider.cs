using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoundCollider : MonoBehaviour
{
    private Monster_temp _monster;
    private bool _soundCool = true;
    private List<Collider> _detectedSounds = new();

    private float _memoryDuration = 3f;        // 기억 유지 시간 (초)
    private float _memoryTimer = 0f;           // 기억 타이머

    private void Awake()
    {
        _monster = GetComponentInParent<Monster_temp>();
    }

    private void Update()
    {
        // 삭제되었거나 비활성화된 감지 대상 제거
        _detectedSounds.RemoveAll(col => col == null || !col.gameObject.activeInHierarchy);

        if (_detectedSounds.Count > 0)
        {
            Collider latest = _detectedSounds[_detectedSounds.Count - 1];
            _memoryTimer += Time.deltaTime;

            // 사운드 오브젝트일 경우에만 코루틴 실행
            if (_soundCool && ((1 << latest.gameObject.layer) & _monster.SoundLayerMask) != 0)
            {
                StartCoroutine(SoundCoolTime(latest));
            }

            // 기억 유지 시간이 초과되면 초기화
            if (_memoryTimer > _memoryDuration)
            {
                _detectedSounds.Clear();
                _monster.IsEventActive = false;
                _monster.TempPoint = Vector3.zero;
                _memoryTimer = 0f;
            }
        }
        else
        {
            _monster.IsEventActive = false;
            _monster.TempPoint = Vector3.zero;
            _memoryTimer = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;

        if (((1 << layer) & (_monster.PlayerLayerMask | _monster.SoundLayerMask)) != 0)
        {
            if (!_detectedSounds.Contains(other))
                _detectedSounds.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int layer = other.gameObject.layer;

        if (((1 << layer) & (_monster.PlayerLayerMask | _monster.SoundLayerMask)) != 0)
        {
            if (_detectedSounds.Contains(other))
                _detectedSounds.Remove(other);
        }
    }

    private IEnumerator SoundCoolTime(Collider latest)
    {
        _soundCool = false;

        // 죽은 상태면 무시
        if (_monster._monsterMerchine.CurState == _monster._monsterMerchine.StateDic[Estate.Dead])
        {
            _soundCool = true;
            yield break;
        }

        int layer = latest.gameObject.layer;

        // 사운드 감지 처리
        if (((1 << layer) & _monster.SoundLayerMask) != 0)
        {
            // 이미 해당 이벤트 위치에 도달한 경우 중복 실행 방지
            if (_monster._monsterMerchine.CurState == _monster._monsterMerchine.StateDic[Estate.GoToEvent])
            {
                float dist = Vector3.Distance(_monster.TempPoint, latest.transform.position);
                if (dist < 0.5f)
                {
                    yield return new WaitForSeconds(0.3f);
                    _soundCool = true;
                    yield break;
                }
            }

            _monster.TempPoint = latest.transform.position;
            _monster.IsEventActive = true;
            _monster.SoundDetectPlayer(latest);
            _monster._monsterMerchine.ChangeState(_monster._monsterMerchine.StateDic[Estate.GoToEvent]);
        }

        // 플레이어 감지 처리 (회전만)
        else if (((1 << layer) & _monster.PlayerLayerMask) != 0)
        {
            _monster.TargetPosition = latest.transform;

            Vector3 lookDir = (_monster.TargetPosition.position - _monster.transform.position).normalized;
            lookDir.y = 0;
            if (lookDir != Vector3.zero)
            {
                _monster.transform.rotation = Quaternion.LookRotation(lookDir);
            }

            Debug.Log("플레이어 감지: 고개만 돌림 (상태 전이 없음)");
        }

        yield return new WaitForSeconds(0.3f);
        _soundCool = true;
    }
}
