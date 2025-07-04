using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : Parameter
{
    public Hunger(float value)
    {
        Init(value);
    }
    
    public override void Act(ref float atkSpeed, float baseValue, float buffValue, float deBuffValue)
    {
        switch (State)
        {
            case ParamState.Full:
                Advantage(ref atkSpeed, buffValue);
                break;
            case ParamState.Basic:
                ResetValue(ref atkSpeed, baseValue);
                break;
            case ParamState.Lack:
                Penalty(ref atkSpeed,deBuffValue);
                break;
            case ParamState.Depletion:
                Penalty(ref atkSpeed, deBuffValue);
                break;
        } 
    }
    
    public override void Advantage(ref float atkSpeed, float buffValue)
    {
        atkSpeed = buffValue;
        
    }
    
    public override void ResetValue(ref float atkSpeed, float baseValue)
    {
        atkSpeed = baseValue;
    }
    
    public override void Penalty(ref float atkSpeed, float deBuffValue)
    {
        atkSpeed = deBuffValue;
    }
}
