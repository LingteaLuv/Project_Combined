using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : Parameter
{
    public Hp(float value)
    {
        Init(value);
    }
    
    public override void Act(ref float atk, float baseValue, float buffValue, float deBuffValue)
    {
        switch (State)
        {
            case ParamState.Full:
                Advantage(ref atk, buffValue);
                break;
            case ParamState.Basic:
                ResetValue(ref atk, baseValue);
                break;
            case ParamState.Lack:
                break;
            case ParamState.Depletion:
                Penalty();
                break;
        } 
    }
    
    public override void Advantage(ref float atk, float buffValue)
    {
        atk = buffValue;
    }
    
    public override void ResetValue(ref float atk, float baseValue)
    {
        atk = baseValue;
    }
    
    public override void Penalty()
    {
        GameManager.Instance.GameOver();
    }
}
