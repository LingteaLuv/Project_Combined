using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : Parameter
{
    public override void Recover(float value)
    {
        Value = Value + value > 100 ? 100 : Value + value;
    }

    public override void Decrease(float value)
    {
        Value = Value - value < 0 ? 0 : Value - value;
    }

    public override void Penalty(ref float atkSpeed, float value)
    {
        atkSpeed = value;
    }

    public override void Reset(ref float atkSpeed, float baseValue)
    {
        atkSpeed = baseValue;
    }
    
    public override void Advantage(ref float atkSpeed, float value)
    {
        atkSpeed = value;
    }
}
