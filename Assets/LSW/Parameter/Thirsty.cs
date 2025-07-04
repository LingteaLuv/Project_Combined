using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thirsty : Parameter
{
    public Thirsty(float value)
    {
        Init(value);
    }
    
    public override void Act(ref float speed, float baseValue, float buffValue, float deBuffValue)
    {
        switch (State)
        {
            case ParamState.Full:
                ResetValue(ref speed, buffValue);
                break;
            case ParamState.Basic:
                ResetValue(ref speed, baseValue);
                break;
            case ParamState.Lack:
                Penalty(ref speed, deBuffValue);
                break;
            case ParamState.Depletion:
                Penalty(ref speed, deBuffValue);
                break;
        } 
    }
    public override void ResetValue(ref float speed, float baseValue)
    {
        speed = baseValue;
    }
    
    public override void Penalty(ref float speed, float deBuffValue)
    {
        speed = deBuffValue;
    }
}
