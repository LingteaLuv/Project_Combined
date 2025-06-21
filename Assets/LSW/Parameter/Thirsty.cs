using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thirsty : Parameter
{
    public override void Recover(float value)
    {
        Value = Value + value > 100 ? 100 : Value + value;
    }

    public override void Decrease(float value)
    {
        Value = Value - value < 0 ? 0 : Value - value;
    }

    public override void Reset(ref float speed, float baseValue)
    {
        speed = baseValue;
    }
    
    public override void Penalty(ref float speed, float value)
    {
        speed = value;
    }
}
