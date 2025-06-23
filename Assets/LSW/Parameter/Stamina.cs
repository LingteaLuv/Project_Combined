using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : Parameter
{
    public bool IsOnPenalty { get; private set; }
    
    public Stamina(float value)
    {
        Init(value);
    }
    
    public override void Act()
    {
        switch (State)
        {
            case ParamState.Full:
                ResetValue();
                break;
            case ParamState.Basic:
                ResetValue();
                break;
            case ParamState.Lack:
                Penalty();
                break;
            case ParamState.Depletion:
                Penalty();
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
