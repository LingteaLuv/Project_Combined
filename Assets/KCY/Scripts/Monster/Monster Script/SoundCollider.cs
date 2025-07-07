using System.Collections;
using UnityEngine;
public class SoundCollider : MonoBehaviour
{
    private Monster_temp _monster;
    private WaitForSeconds _delay;
    private Coroutine _detectCor;
    private float _detectTime;
    
    private void Awake()
    {
        _monster = GetComponentInParent<Monster_temp>();
        _delay = new WaitForSeconds(0.1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _monster.SoundDetectPlayer(other);
            _monster.IsEventActive = true;
            _detectTime = Time.time;
        }
    }
    /*private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & _monster.PlayerLayerMask) != 0 ||
            ((1 << other.gameObject.layer) & _monster.SoundLayerMask) != 0)
        {
            _monster.IsEventActive = true;
        }
    }*/

    private void Update()
    {
        if (_detectCor == null)
        {
            _detectCor = StartCoroutine(DetectPlayer());
        }

        /*if (_monster.IsEventActive && Time.time - _detectTime > 1f)
        {
            _monster.IsEventActive = true;
        }*/
    }
    
    private IEnumerator DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _monster.Info.HearingRange, _monster.PlayerLayerMask);
        foreach (var hit in hits)
        {
            _monster.IsEventActive = true;
        }

        yield return _delay;
        _detectCor = null;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
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