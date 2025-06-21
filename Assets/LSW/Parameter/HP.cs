using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : Parameter
{
    public override void Act(ref float atk, float baseValue, float offset)
    {
        switch (State)
        {
            case ParamState.Full:
                Advantage(ref atk, baseValue, offset);
                break;
            case ParamState.Basic:
                ResetValue(ref atk, baseValue, offset);
                break;
            case ParamState.Lack:
                break;
            case ParamState.Depletion:
                Penalty();
                break;
        } 
    }
    
    public override void Advantage(ref float atk, float baseValue, float offset)
    {
        atk = baseValue + offset;
    }
    
    public override void ResetValue(ref float atk, float baseValue, float offset)
    {
        atk = baseValue;
    }
    
    public override void Penalty()
    {
        GameManager.Instance.GameOver();
    }
}
