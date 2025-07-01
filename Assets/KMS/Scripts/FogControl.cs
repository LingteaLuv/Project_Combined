using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogControl : MonoBehaviour
{
    [SerializeField] Animator _anim;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home)) SetFogTrigger();
    }

    public void SetFogTrigger()
    {
        _anim.SetTrigger("FogToggle");
    }
}
