using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thirsty : Parameter
{
    public override void Act(ref float speed, float baseValue, float offset)
    {
        switch (State)
        {
            case ParamState.Full:
                break;
            case ParamState.Basic:
                ResetValue(ref speed, baseValue, offset);
                break;
            case ParamState.Lack:
                Penalty(ref speed, baseValue, offset);
                break;
            case ParamState.Depletion:
                break;
        } 
    }
    public override void ResetValue(ref float speed, float baseValue, float offset)
    {
        speed = baseValue;
    }
    
    public override void Penalty(ref float speed, float baseValue, float offset)
    {
        speed = baseValue - offset;
    }
}
