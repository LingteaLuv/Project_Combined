using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : Parameter
{
    public override void Recover(float value)
    {
        Value = Value + value > 100 ? 100 : Value + value;
    }

    public override void Decrease(float value)
    {
        Value = Value - value < 0 ? 0 : Value - value;
    }

    public override void Penalty()
    {
        GameManager.Instance.GameOver();
    }

    public override void Reset(ref float atk, float baseValue)
    {
        atk = baseValue;
    }
    
    public override void Advantage(ref float atk, float value)
    {
        atk = value;
    }
}
