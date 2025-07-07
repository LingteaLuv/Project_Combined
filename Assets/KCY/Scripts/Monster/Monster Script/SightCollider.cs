using System.Collections;
using UnityEngine;

/*
public class SightCollider : MonoBehaviour
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
        //Debug.Log("SightCollider.OnTriggerEnter 호출됨");
        _monster.SightDetectPlayer(other);
        _detectTime = Time.time;
    }

    private void Update()
    {
        if (_detectCor == null)
        {
            _detectCor = StartCoroutine(DetectPlayer());
        }
        
        /*if (_monster.IsEventActive && Time.time - _detectTime > 1f)
        {
            _monster.IsEventActive = true;
        }#1#
    }
    
    private IEnumerator DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _monster.Info.HearingRange, _monster.PlayerLayerMask);
        foreach (var hit in hits)
        {
            _monster.SightDetectPlayer(hit);
        }

        yield return _delay;
        _detectCor = null;
    }
    
    /*private void OnTriggerStay(Collider other)
    {
        _monster.SightDetectPlayer(other);
    }#1#
    
    private void OnTriggerExit(Collider other)
    {
        // 플레이어 아니면 나가고
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        if (_monster.IsDetecting && _monster.TargetPosition == other.transform)
        {
            //Debug.Log(" 플레이어 감지 해제 Idle 상태 복귀");
            //Debug.Log(" 제발 돌아와 이 ㅆ");
          
            _monster.IsDetecting = false;
            _monster.IsSightDetecting = false;

            // 감지해서 나간 대상이 플레이어였을 경우 해당 위치를 임시 포인트로 잡고 해당 방향을 기준으로 패트롤을 진행한다.
            _monster.TempPoint = _monster.transform.position;
            _monster._monsterMerchine.ChangeState(_monster._monsterMerchine.StateDic[Estate.Idle]);
        }
    }

}
*/
