using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HpParameter : MonoBehaviour
{
    public Property<int> Hp;

    private void Update()
    {
        if (Hp.Value == 100)
        {
            Advantage();
        }
        else if (Hp.Value > 10 && Hp.Value < 40)
        {
            Penalty();
        }
        else
        {
            
        }
    }

    public void Recover()
    {
        Hp.Value++;
    }

    public void Decrease()
    {
        Hp.Value--;
    }

    public void Penalty()
    {
        // 패널티 메서드
    }

    public void Advantage()
    {
        // 어드밴티지 메서드
    }

    private void Init()
    {
        
    }
}
