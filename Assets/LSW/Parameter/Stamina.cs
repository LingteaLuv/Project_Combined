using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : Parameter
{
    public bool IsOnPenalty { get; private set; }
    
    public override void Act()
    {
        switch (State)
        {
            case ParamState.Full:
                break;
            case ParamState.Basic:
                ResetValue();
                break;
            case ParamState.Lack:
                Penalty();
                break;
            case ParamState.Depletion:
                break;
        } 
    }
    public override void ResetValue()
    {
        IsOnPenalty = false;
    }
    
    public override void Penalty()
    {
        IsOnPenalty = true;
    }
}
