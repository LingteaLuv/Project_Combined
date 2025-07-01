using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogControl : MonoBehaviour
{
    [SerializeField] Animator _anim;
    private void Start()
    {
        StartCoroutine(Delay());
    }

    private void FogUpdate(DayTime time)
    {
        Debug.Log(time);
        if (time == DayTime.MidNight || time == DayTime.Morning)
        {
            SetFogTrigger();
        }
    }

    public void SetFogTrigger()
    {
        _anim.SetTrigger("FogToggle");
    }
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        TimeManager.Instance.CurrentTimeOfDay.OnChanged += FogUpdate;
    }
}
