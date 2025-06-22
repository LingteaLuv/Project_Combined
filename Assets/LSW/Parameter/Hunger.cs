using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : Parameter
{
    public override void Act(ref float atkSpeed, float baseValue, float offset)
    {
        switch (State)
        {
            case ParamState.Full:
                Advantage(ref atkSpeed, baseValue, offset);
                break;
            case ParamState.Basic:
                ResetValue(ref atkSpeed, baseValue, offset);
                break;
            case ParamState.Lack:
                Penalty(ref atkSpeed, baseValue, offset);
                break;
            case ParamState.Depletion:
                break;
        } 
    }
    
    public override void Advantage(ref float atkSpeed, float baseValue, float offset)
    {
        atkSpeed = baseValue + offset;
    }
    
    public override void ResetValue(ref float atkSpeed, float baseValue, float offset)
    {
        atkSpeed = baseValue;
    }
    
    public override void Penalty(ref float atkSpeed, float baseValue, float offset)
    {
        atkSpeed = baseValue - offset;
    }
}
