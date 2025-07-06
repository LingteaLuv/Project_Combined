using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoundCollider : MonoBehaviour
{
    private Monster_temp _monster;
    private bool _soundCool = true;
    private List<Collider> _detectedSounds = new();

    private void Awake()
    {
        _monster = GetComponentInParent<Monster_temp>();
    }

    private void Update()
    {
        // 리스트에서 삭제된 사운드 제거
        _detectedSounds.RemoveAll(col => col == null || !col.gameObject.activeInHierarchy);

        // 가장 최신 감지 오브젝트 기준으로 갱신
        if (_detectedSounds.Count > 0)
        {
            Collider latest = _detectedSounds[_detectedSounds.Count - 1];
            if (_soundCool)
            {
                StartCoroutine(SoundCoolTime(latest));
            }
        }
        else
        {
            _monster.IsEventActive = false;
            _monster.TempPoint = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & (_monster.PlayerLayerMask | _monster.SoundLayerMask)) != 0)
        {
            if (!_detectedSounds.Contains(other))
                _detectedSounds.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & (_monster.PlayerLayerMask | _monster.SoundLayerMask)) != 0)
        {
            if (_detectedSounds.Contains(other))
                _detectedSounds.Remove(other);
        }
    }

    private IEnumerator SoundCoolTime(Collider latest)
    {
        _soundCool = false;

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
        _monster.SoundDetectPlayer(latest);
        _monster.IsEventActive = true;

        _monster._monsterMerchine.ChangeState(_monster._monsterMerchine.StateDic[Estate.GoToEvent]);

        yield return new WaitForSeconds(0.3f);
        _soundCool = true;
    }
       
}
